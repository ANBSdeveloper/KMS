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
    public class CustomerGetListByStaffHandler : QueryHandlerBase, IRequestHandler<CustomerGetListByStaff, PagingResult<CustomerByStaffListDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CustomerGetListByStaffHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<CustomerByStaffListDto>> Handle(CustomerGetListByStaff request, CancellationToken cancellationToken)
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
            else
            {
                staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
                if (staff != null)
                {
                    salesOrgId = staff.SalesOrgId;
                }
            }

            bool isActive = request.IsActive ?? true;
            int fromNumber = request.Skip.HasValue ? request.Skip.Value + 1 : 1;
            int toNumber = fromNumber + (request.MaxResult.HasValue ? request.MaxResult.Value : 0) - 1;
            var sqlSort = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("c", request.Sort) : "c.Code";

            string cteSql;

            if (salesOrgId.HasValue)
            {
                cteSql = @$"
                        SELECT SalesOrgs.*
                        FROM   SalesOrgs
                	    WHERE Id = {salesOrgId}";
            }
            else
            {
                cteSql = @$"
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
                                SELECT 
                                    c.*,
                                    ROW_NUMBER() OVER (ORDER BY {sqlSort}) AS RowNumber
                                INTO #Customer
                                FROM  (
                                    SELECT c.*, 
                                        ZoneName = ISNULL(z.Name,''), 
                                        AreaName = ISNULL(ar.Name, ''),
                                        WardName = ISNULL(w.Name,''), 
                                        DistrictName = ISNULL(d.Name,''), 
                                        ProvinceName = ISNULL(p.Name,''), 
                                        SalesSupervisorStaffCode = ISNULL(s.Code,''), SalesSupervisorStaffName = ISNULL(s.Name,'')
                                    FROM Customers AS c
                                        LEFT JOIN Zones z ON c.ZoneId = z.Id
                					    LEFT JOIN Areas ar ON c.AreaId = ar.Id
					                    LEFT JOIN Wards w ON c.WardId = w.Id
					                    LEFT JOIN Districts d ON c.DistrictId = d.Id
					                    LEFT JOIN Provinces p ON c.ProvinceId = p.Id
                                        LEFT JOIN Staffs s ON c.SalesSupervisorStaffId = s.Id
                                    WHERE EXISTS(SELECT TOP 1 *
                                        FROM CTE
                                        INNER JOIN Branches AS b ON b.SalesOrgId = CTE.Id
                                        WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId) "
                                    + (request.KeyShopStatus.Count > 0 ? $@"AND c.KeyShopStatus IN @KeyShopStatus " : "")
                                    + (request.ProvinceId.HasValue ? $@"AND c.ProvinceId = {request.ProvinceId} " : "")
                                    + (request.DistrictId.HasValue ? $@"AND c.DistrictId = {request.DistrictId} " : "")
                                    + (request.WardId.HasValue ? $@"AND c.WardId = {request.WardId} " : "")
                                    + (request.IsActive.HasValue ? $@"AND c.IsActive = {(isActive ? "1" : "0")} " : "")
                                    + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                                    + (request.HasTicketInvestment == true ? $@"AND EXISTS(
                                        SELECT TOP 1 * 
                                        FROM TicketInvestments AS ticketInvestment
                                        WHERE ticketInvestment.CustomerId = c.Id
                                    ) " : "")
                                    + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                                                    c.Code LIKE N'%{request.Keyword}%' OR
                                                    c.Name LIKE N'%{request.Keyword}%' OR
                                                    c.Address LIKE N'%{request.Keyword}%' OR
                                                    c.Phone LIKE N'%{request.Keyword}%' OR
                                                    c.MobilePhone LIKE N'%{request.Keyword}%') " : "")
                                + ") AS c "
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
                                + (request.ProvinceId.HasValue ? $@"AND c.ProvinceId = { request.ProvinceId} " : "")
                                + (request.DistrictId.HasValue ? $@"AND c.DistrictId = {request.DistrictId} " : "")
                                + (request.WardId.HasValue ? $@"AND c.WardId = {request.WardId} " : "")
                                + (request.IsActive.HasValue ? $@"AND c.IsActive = {(isActive ? "1" : "0")} " : "")
                                + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                                + (request.HasTicketInvestment == true ? $@"AND EXISTS(
                                        SELECT TOP 1 * 
                                        FROM TicketInvestments AS ticketInvestment
                                        WHERE ticketInvestment.CustomerId = c.Id
                                    ) " : "")
                                + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                                                                c.Code LIKE N'%{request.Keyword}%' OR
                                                                c.Name LIKE N'%{request.Keyword}%' OR
                                                                c.Address LIKE N'%{request.Keyword}%' OR
                                                                c.Phone LIKE N'%{request.Keyword}%' OR
                                                                c.MobilePhone LIKE N'%{request.Keyword}%') " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<CustomerByStaffListDto>(pagingSql, new { 
                KeyShopStatus = request.KeyShopStatus
            });
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new
            {
                KeyShopStatus = request.KeyShopStatus
            });

            return new PagingResult<CustomerByStaffListDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
        }
    }
}