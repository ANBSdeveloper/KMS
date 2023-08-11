using Cbms.Domain.Entities;
using Cbms.Extensions;
using Cbms.Kms.Application.Orders.Dto;
using Cbms.Kms.Application.Orders.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Orders.QueryHandler
{
    public class OrderGetListHandler : QueryHandlerBase, IRequestHandler<OrderGetList, PagingResult<OrderListItemDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public OrderGetListHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }       

        public async Task<PagingResult<OrderListItemDto>> Handle(OrderGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            string sql = string.Empty;
            int? salesOrgId = null;
            Staff staff;
            if (request.FromDate == System.DateTime.MinValue || request.ToDate == System.DateTime.MinValue)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("FilterDateInvalid").Build();
            }
            if (request.SalesSupervisorStaffId.HasValue)
            {
                staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.Id == request.SalesSupervisorStaffId);
                if (staff != null)
                {
                    salesOrgId = staff.SalesOrgId;
                }
                else
                {
                    throw new EntityNotFoundException(typeof(Staff), request.SalesSupervisorStaffId);
                }
            }
            else if (request.AsmStaffId.HasValue)
            {
                staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.Id == request.AsmStaffId);
                if (staff != null)
                {
                    salesOrgId = staff.SalesOrgId;
                }
                else
                {
                    throw new EntityNotFoundException(typeof(Staff), request.AsmStaffId);
                }
            }
            else if (request.RsmStaffId.HasValue)
            {
                staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.Id == request.RsmStaffId);
                if (staff != null)
                {
                    salesOrgId = staff.SalesOrgId;
                }
                else
                {
                    throw new EntityNotFoundException(typeof(Staff), request.RsmStaffId);
                }
            }
            else // Tất cả theo user đăng nhập
            {
                staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
                if (staff != null)
                {
                    salesOrgId = staff.SalesOrgId;
                }
            }

            string cteSql;

            if (salesOrgId.HasValue)
            {
                cteSql = $@"
                    SELECT SalesOrgs.*
                    FROM   SalesOrgs
	                WHERE Id = {salesOrgId}";
            }
            else
            {
                cteSql = $@"
                    SELECT SalesOrgs.*
                    FROM   SalesOrgs
                    INNER JOIN UserAssignments  ON SalesOrgs.Id = UserAssignments.SalesOrgId
	                WHERE UserAssignments.UserId = {Session.UserId}";
            }
           
            int fromNumber = request.Skip.HasValue ? request.Skip.Value + 1 : 1;
            int toNumber = fromNumber + (request.MaxResult.HasValue ? request.MaxResult.Value : 0) - 1;
            var sqlSort = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("i", request.Sort) : "i.OrderDate";

            string pagingSql = $@"
                    IF OBJECT_ID('tempdb..#Order') IS NOT NULL
                    DROP TABLE #Order;

                    WITH CTE AS
                    (
                        {cteSql}

                        UNION ALL

                        SELECT SalesOrgs.*
                        FROM   SalesOrgs
                        INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                    )
                    SELECT
                        i.*,
                        ROW_NUMBER() OVER (ORDER BY {sqlSort}) AS RowNumber
                    INTO #Order
                    FROM (
                        SELECT
                            i.*,
                            CustomerName = c.Name,
                            CustomerCode = c.Code,
                            TicketInvestmentCode = t.Code,
                            TicketCodes =  STUFF(
                                (   SELECT ', ' + ticket.Code 
                                    FROM OrderTickets AS orderTicket
                                    JOIN Tickets AS ticket ON orderTicket.TicketId = ticket.Id
                                    WHERE orderTicket.OrderId = i.Id
                                    FOR XML PATH ('')
                                ), 1, 2, ''
                            )
                        FROM Orders AS i
                        INNER JOIN Customers AS c ON i.CustomerId = c.Id
                        INNER JOIN TicketInvestments AS t ON i.TicketInvestmentId = t.Id
                        WHERE EXISTS(SELECT TOP 1 *
                            FROM CTE
                            INNER JOIN Branches AS b ON b.SalesOrgId = CTE.Id
                            WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId)
                        AND i.OrderDate >= @FromDate AND i.OrderDate <= @ToDate "
                        + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                        + (request.CustomerId.HasValue ? $@"AND i.CustomerId = { request.CustomerId} " : "")
                        + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                                        i.OrderNumber LIKE N'%{request.Keyword}%' OR
                                        i.ConsumerPhone LIKE N'%{request.Keyword}%' OR
                                        i.ConsumerName LIKE N'%{request.Keyword}%' OR
                                        c.Name LIKE N'%{request.Keyword}%' OR
                                        c.Code LIKE N'%{request.Keyword}%') " : "")
                    + @$") AS i "
                    + @$"SELECT * FROM #Order
                    WHERE RowNumber >= {fromNumber} "
                    + (request.MaxResult.HasValue ? @$"AND RowNumber <= {toNumber}" : "");

            string countSql = $@"
                WITH CTE AS
                (
                    {cteSql}

                    UNION ALL

                    SELECT SalesOrgs.*
                    FROM   SalesOrgs
                    INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                )
                SELECT COUNT(*)
                FROM Orders AS i
                INNER JOIN Customers AS c ON i.CustomerId = c.Id
                WHERE EXISTS(SELECT TOP 1 *
                                FROM CTE
                                INNER JOIN Branches AS b ON b.SalesOrgId = cTE.Id
                                WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId)
                AND i.OrderDate >= @FromDate AND i.OrderDate <= @ToDate "
                + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                + (request.CustomerId.HasValue ? $@"AND i.CustomerId = { request.CustomerId} " : "")
                        + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                                    i.OrderNumber LIKE N'%{request.Keyword}%' OR
                                    i.ConsumerPhone LIKE N'%{request.Keyword}%' OR
                                    i.ConsumerName LIKE N'%{request.Keyword}%' OR
                                    c.Name LIKE N'%{request.Keyword}%' OR
                                    c.Code LIKE N'%{request.Keyword}%') " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<OrderListItemDto>(pagingSql, new
            {
                FromDate = request.FromDate.BeginOfDay(),
                ToDate = request.ToDate.EndOfDay()
            });
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new
            {
                FromDate = request.FromDate.BeginOfDay(),
                ToDate = request.ToDate.EndOfDay()
            });

            return new PagingResult<OrderListItemDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
            
        }
    }
}