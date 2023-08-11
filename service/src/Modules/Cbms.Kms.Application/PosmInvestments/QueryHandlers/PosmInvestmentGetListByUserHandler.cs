using Cbms.Domain.Entities;
using Cbms.Extensions;
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
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.QueryHandler
{
    public class PosmInvestmentGetListByUserHandler : QueryHandlerBase, IRequestHandler<PosmInvestmentGetListByUser, PagingResult<PosmInvestmentListDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public PosmInvestmentGetListByUserHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<PosmInvestmentListDto>> Handle(PosmInvestmentGetListByUser request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            int? salesOrgId = null;
            Staff staff;
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.EndOfDay();

            var setting = await _dbContext.InvestmentSetting.FirstOrDefaultAsync();
            if (setting != null)
            {
                int month = Convert.ToInt32(setting.MaxInvestmentQueryMonths);
                fromDate = fromDate.AddMonths(-month);
            }


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
            if (salesOrgId.HasValue)
            {
                cteSql = $@"
                    SELECT SalesOrgs.*
                    FROM   SalesOrgs
	                WHERE Id = {salesOrgId} ";
            }
            else
            {
                cteSql = $@"
                    SELECT SalesOrgs.*
                    FROM   SalesOrgs
                    INNER JOIN UserAssignments  ON SalesOrgs.Id = UserAssignments.SalesOrgId
	                WHERE UserAssignments.UserId = {Session.UserId} ";
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
                                WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId) "
                        + (request.CycleId.HasValue ? $@"AND i.CycleId = {request.CycleId} " : "")
                        + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                        + (request.Status.Count > 0 ? $@"AND (i.Status IN @Status OR EXISTS(
                                SELECT * 
                                FROM PosmInvestmentItems AS iv 
                                WHERE iv.PosmInvestmentId = i.Id 
                                AND iv.Status IN @Status)) " : "")
                        + (request.WardId.HasValue ? $@"AND c.WardId = @WardId " : "")
                        + (request.DistrictId.HasValue ? $@"AND c.DistrictId = @DistrictId " : "")
                        + (request.ProvinceId.HasValue ? $@"AND c.ProvinceId = @ProvinceId " : "")
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
                                WHERE CTE.TypeId = 1146 AND b.Id = c.BranchId) "
                + (request.CycleId.HasValue ? $@"AND i.CycleId = {request.CycleId} " : "")
                + (salesOrgId.HasValue ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "")
                + (request.Status.Count > 0 ? $@"AND (i.Status IN @Status OR EXISTS(
                                SELECT * 
                                FROM PosmInvestmentItems AS iv 
                                WHERE iv.PosmInvestmentId = i.Id 
                                AND iv.Status IN @Status)) " : "")
                + (request.WardId.HasValue ? $@"AND c.WardId = @WardId " : "")
                + (request.DistrictId.HasValue ? $@"AND c.DistrictId = @DistrictId " : "")
                + (request.ProvinceId.HasValue ? $@"AND c.ProvinceId = @ProvinceId " : "")
                + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                        i.Code LIKE N'%{request.Keyword}%' OR 
                        c.Name LIKE N'%{request.Keyword}%' OR 
                        c.Code LIKE N'%{request.Keyword}%' OR 
                        c.MobilePhone LIKE N'%{request.Keyword}%') " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<PosmInvestmentListDto>(pagingSql, new { fromDate, toDate, Status = request.Status, request.WardId, request.DistrictId, request.ProvinceId });
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new { fromDate, toDate, Status = request.Status });

            return new PagingResult<PosmInvestmentListDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
            
        }
    }
}