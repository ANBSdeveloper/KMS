using Cbms.Domain.Entities;
using Cbms.Kms.Application.Customers.Dto;
using Cbms.Kms.Application.Customers.Query;
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

namespace Cbms.Kms.Application.Customers.QueryHandler
{
    public class CustomerGetListHandler : QueryHandlerBase, IRequestHandler<CustomerGetList, PagingResult<CustomerDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CustomerGetListHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<CustomerDto>> Handle(CustomerGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            string sql = string.Empty;
            int? salesOrgId = null;
            Staff staff;
            if (request.StaffId.HasValue)
            {
                staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.Id == request.StaffId);
                if (staff != null)
                {
                    salesOrgId = staff.SalesOrgId;
                }
                else
                {
                    throw new EntityNotFoundException(typeof(Staff), request.StaffId);
                }
            }
            else
            {
                staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
                if (staff != null)
                {
                    salesOrgId = staff.SalesOrgId;
                }
            }
            string cteSql = "";
            bool isActive = request.IsActive ?? true;
            int fromNumber = request.Skip.HasValue ? request.Skip.Value + 1 : 1;
            int toNumber = fromNumber + (request.MaxResult.HasValue ? request.MaxResult.Value : 0) - 1;
            var sqlSort = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("c", request.Sort) : "c.Code";
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
                        SELECT c.*, ROW_NUMBER() OVER (ORDER BY {sqlSort}) AS RowNumber
                        INTO #Customer
                        FROM Customers AS c
                        WHERE EXISTS(SELECT TOP 1 *
                            FROM CTE
                            INNER JOIN Branches AS b ON b.SalesOrgId = CTE.Id
                            WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId) "
                        + (request.KeyShopStatus.Count > 0 ? $@"AND c.KeyShopStatus IN @KeyShopStatus " : "")
                        + (request.IsKeyShop.Value ? $@"AND c.IsKeyShop = 1 " : "")
                        + (request.ProvinceId.HasValue ? $@"AND c.ProvinceId = {request.ProvinceId} " : "")
                        + (request.DistrictId.HasValue ? $@"AND c.DistrictId = {request.DistrictId} " : "")
                        + (request.WardId.HasValue ? $@"AND c.WardId = {request.WardId} " : "")
                        + (request.IsActive.HasValue ? $@"AND c.IsActive = {(isActive ? "1" : "0")} " : "")
                        + (request.HasTicketInvestment == true ? $@"AND EXISTS(
                                        SELECT TOP 1 * 
                                        FROM TicketInvestments AS ticketInvestment
                                        WHERE ticketInvestment.CustomerId = c.Id
                                    ) " : "")
                        + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                        + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                            c.Code LIKE N'%{request.Keyword}%' OR
                            c.Name LIKE N'%{request.Keyword}%' OR
                            c.Address LIKE N'%{request.Keyword}%' OR
                            c.Phone LIKE N'%{request.Keyword}%' OR
                            c.MobilePhone LIKE N'%{request.Keyword}%') " : "")
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
                    FROM Customers AS c
                    WHERE EXISTS(SELECT TOP 1 *
                            FROM CTE
                            INNER JOIN Branches AS b ON b.SalesOrgId = CTE.Id
                            WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId) "
                    + (request.KeyShopStatus.Count > 0 ? $@"AND c.KeyShopStatus IN @KeyShopStatus " : "")
                    + (request.IsKeyShop.Value ? $@"AND c.IsKeyShop = 1 " : "")
                    + (request.ProvinceId.HasValue ? $@"AND c.ProvinceId = { request.ProvinceId} " : "")
                    + (request.DistrictId.HasValue ? $@"AND c.DistrictId = {request.DistrictId} " : "")
                    + (request.WardId.HasValue ? $@"AND c.WardId = {request.WardId} " : "")
                    + (request.IsActive.HasValue ? $@"AND c.IsActive = {(isActive ? "1" : "0")} " : "")
                    + (request.HasTicketInvestment == true ? $@"AND EXISTS(
                                        SELECT TOP 1 * 
                                        FROM TicketInvestments AS ticketInvestment
                                        WHERE ticketInvestment.CustomerId = c.Id
                                    ) " : "")
                    + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                    + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                            c.Code LIKE N'%{request.Keyword}%' OR
                            c.Name LIKE N'%{request.Keyword}%' OR
                            c.Address LIKE N'%{request.Keyword}%' OR
                            c.Phone LIKE N'%{request.Keyword}%' OR
                            c.MobilePhone LIKE N'%{request.Keyword}%') " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<CustomerDto>(pagingSql, new
            {
                KeyShopStatus = request.KeyShopStatus
            });
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new
            {
                KeyShopStatus = request.KeyShopStatus
            });

            return new PagingResult<CustomerDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
        }
    }
}