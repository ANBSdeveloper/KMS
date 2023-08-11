using Cbms.Domain.Entities;
using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments.Query;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.QueryHandler
{
    public class PosmInvestmentGetListByTimeHandler : QueryHandlerBase, IRequestHandler<PosmInvestmentGetListByTime, PagingResult<PosmInvestmentListDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public PosmInvestmentGetListByTimeHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<PosmInvestmentListDto>> Handle(PosmInvestmentGetListByTime request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            string sql = string.Empty;
            int? salesOrgId = null;
            Staff staff;
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
                // Trường hợp sup vừa team lead
                if (Session.Roles.Contains(KmsConsts.CustomerDevelopmentLeadRole))
                {
                    staff = null;
                }

                if (staff != null)
                {
                    salesOrgId = staff.SalesOrgId;
                }
            }

            string cteSql = "";
            if (salesOrgId.HasValue)
            {
                cteSql = $@"
                    SELECT SalesOrgs.*
                    FROM   SalesOrgs
	                WHERE Id = {salesOrgId}
                ";
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
            var sqlSort = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("i", request.Sort) : "i.CreationTime";
            string pagingSql = $@"

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
                        SELECT 
                            i.*,
                            ROW_NUMBER() OVER (ORDER BY {sqlSort}) AS RowNumber
                        INTO #Customer
                        FROM  (
                            SELECT
                                i.*,
                                CustomerCode = c.Code,
                                CustomerName = c.Name,
                                Email = c.Email,
                                MobilePhone = c.MobilePhone,
                                Address = c.Address,
                                RegisterStaffName = s.Name,
                                ZoneName = z.Name,
					            AreaName = ar.Name
                            FROM PosmInvestments AS i
                            INNER JOIN Customers AS c ON i.CustomerId = c.Id
                            INNER JOIN Zones AS z ON c.ZoneId = z.Id
                            INNER JOIN Areas AS ar ON c.AreaId = ar.Id
                            INNER JOIN Staffs AS s ON i.RegisterStaffId = s.Id
                            WHERE EXISTS(SELECT TOP 1 *
                                FROM CTE
                                INNER JOIN Branches AS b ON b.SalesOrgId = CTE.Id
                                WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId) 
                            AND i.RegisterDate BETWEEN @FromDate AND @ToDate "
                        + (staff != null ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                        + (request.Status.Count > 0 ? $@"AND i.Status IN @Status " : "")
                        + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                                i.Code LIKE N'%{request.Keyword}%' OR
                                c.Name LIKE N'%{request.Keyword}%' OR
                                c.Code LIKE N'%{request.Keyword}%' OR
                                c.MobilePhone LIKE N'%{request.Keyword}%') " : "")
                    + ") AS i "
                    + @$"SELECT * FROM #Customer
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
                    FROM PosmInvestments AS i
                    INNER JOIN Customers AS c ON i.CustomerId = c.Id
                    WHERE EXISTS(SELECT TOP 1 *
                                    FROM CTE
                                    INNER JOIN Branches AS b ON b.SalesOrgId = cTE.Id
                                    WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId) 
                    AND i.RegisterDate BETWEEN @FromDate AND @ToDate "
                + (staff != null ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                + (request.Status.Count > 0 ? $@"AND i.Status IN @Status " : "")
                + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                            i.Code LIKE N'%{request.Keyword}%' OR
                            c.Name LIKE N'%{request.Keyword}%' OR
                            c.Code LIKE N'%{request.Keyword}%' OR
                            c.MobilePhone LIKE N'%{request.Keyword}%') " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<PosmInvestmentListDto>(pagingSql, new
            {
                request.FromDate,
                request.ToDate,
                Status = request.Status
            });
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new
            {
                request.FromDate,
                request.ToDate,
                Status = request.Status
            });

            return new PagingResult<PosmInvestmentListDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
        }
    }
}