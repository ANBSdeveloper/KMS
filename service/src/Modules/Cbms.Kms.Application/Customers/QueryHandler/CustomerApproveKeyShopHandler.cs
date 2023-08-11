using Cbms.Collections.Extensions;
using Cbms.Kms.Application.Customers.Dto;
using Cbms.Kms.Application.Customers.Query;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.QueryHandler
{
    public class CustomerApproveKeyShopHandler : QueryHandlerBase, IRequestHandler<CustomerGetListApproveKeyShop, PagingResult<CustomerApproveKeyShopListDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public CustomerApproveKeyShopHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<PagingResult<CustomerApproveKeyShopListDto>> Handle(CustomerGetListApproveKeyShop request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            string sql = string.Empty;
            int? salesOrgId = null;

            var staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
            if (staff != null)
            {
                salesOrgId = staff.SalesOrgId;
            }

            string cteSql;
            if (salesOrgId != null)
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

            sql = $@"
                    IF OBJECT_ID('tempdb..#Customer') IS NOT NULL
                    DROP TABLE #Customer;

                    WITH CTE AS
                    (
                        {cteSql}

                        UNION ALL

                        SELECT SalesOrgs.*
                        FROM   SalesOrgs
                        INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                    )
                    SELECT IsSelected = CAST(0 as BIT), c.Code, c.Name, c.MobilePhone, c.Birthday, c.Address
				    ,c.KeyShopStatus, c.KeyShopRegisterStaffId, StaffName = s.Name,c.Id, c.ChannelCode,c.ChannelName,c.Email, c.KeyShopAuthCode,c.ZoneId, c.AreaId
				    FROM Customers AS c
				    LEFT JOIN Staffs as s ON s.Id = c.KeyShopRegisterStaffId
                    WHERE EXISTS (SELECT TOP 1 * 
                                    FROM CTE 
                                    INNER JOIN Branches AS b ON b.SalesOrgId = cTE.Id
                                    WHERE TypeId = 1146 AND b.Id = c.BranchId)
                    AND c.KeyShopStatus IN @KeyShopStatus "
                    + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                    + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND(
                        c.Code LIKE N'%{request.Keyword}%' OR
                        c.Name LIKE N'%{request.Keyword}%' OR
                        c.Address LIKE N'%{request.Keyword}%' OR
                        c.Phone LIKE N'%{request.Keyword}%' OR
                        s.Name LIKE N'%{request.Keyword}%' OR
                        c.Birthday LIKE N'%{request.Keyword}%' OR
                        c.ChannelName LIKE N'%{request.Keyword}%' OR
                        c.Email LIKE N'%{request.Keyword}%' OR
                        c.KeyShopAuthCode LIKE N'%{request.Keyword}%' OR
                        c.MobilePhone LIKE N'%{request.Keyword}%') " : "") +
                    " ORDER BY c.Code";

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var query = await connection.QueryAsync<CustomerApproveKeyShopListDto>(sql, new { KeyShopStatus = new List<KeyShopStatus>() { KeyShopStatus.Created, KeyShopStatus.Approved, KeyShopStatus.Refuse } });


            query = query
                    .WhereIf(request.ZoneId != 0 ,x => x.ZoneId == request.ZoneId)
                    .WhereIf(request.AreaId != 0, x => x.AreaId == request.AreaId);

            int totalCount = query.Count();
            query = query.SortFromString(request.Sort);
            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<CustomerApproveKeyShopListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}
