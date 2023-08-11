using Cbms.Domain.Entities;
using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Kms.Application.Staffs.Query;
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

namespace Cbms.Kms.Application.Staffs.QueryHandler
{
    public class StaffGetListByRoleHandler : QueryHandlerBase, IRequestHandler<StaffGetListByRole, PagingResult<StaffListDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public StaffGetListByRoleHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<StaffListDto>> Handle(StaffGetListByRole request, CancellationToken cancellationToken)
        {   
            if (request.SupervisorId.HasValue)
            {
                var supervisorStaff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.Id == request.SupervisorId);
                if (supervisorStaff == null)
                {
                    throw new EntityNotFoundException(typeof(Staff), request.SupervisorId);
                }
                else
                {
                    return await GetItemsFromSalesOrgAsync(request, supervisorStaff.SalesOrgId);
                }
            }
            else
            {
                var staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
                if (staff != null)
                {
                    return await GetItemsFromSalesOrgAsync(request, staff.SalesOrgId);
                }

                return await GetItemsFromUserAsync(request);
            }
         
        }

        private async Task<PagingResult<StaffListDto>> GetItemsFromSalesOrgAsync(StaffGetListByRole request, int salesOrgId)
        {
            int fromNumber = request.Skip.HasValue ? request.Skip.Value + 1 : 1;
            int toNumber = fromNumber + (request.MaxResult.HasValue ? request.MaxResult.Value : 0) - 1;
            string sortSql = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("s", request.Sort) : "s.Code";


            string pagingSql = $@"
                        IF OBJECT_ID('tempdb..#Staff') IS NOT NULL
                        DROP TABLE #Staff;

                        WITH CTE AS
                        (
                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
	                        WHERE Id = {salesOrgId}

                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                        )
                        SELECT s.*, ROW_NUMBER() OVER (ORDER BY {sortSql}) AS RowNumber INTO #Staff 
                        FROM Staffs AS s
                        WHERE s.StaffTypeCode = '{request.StaffTypeCode}' 
                        AND s.SalesOrgId IN (
                            SELECT CTE.Id FROM CTE
                        )"
                        + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                            s.Code LIKE N'%{request.Keyword}%' OR 
                            s.Name LIKE N'%{request.Keyword}%') " : "")
                        + @$"SELECT * FROM #Staff
                        WHERE RowNumber >= {fromNumber} "
                        + (request.MaxResult.HasValue ? @$"AND RowNumber <= {toNumber}" : "");

            string countSql = $@"
                     WITH CTE AS
                        (
                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
	                        WHERE Id = {salesOrgId}

                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                        )
                        SELECT COUNT(*)
                        FROM Staffs AS s
                        WHERE s.StaffTypeCode = '{request.StaffTypeCode}' 
                        AND s.SalesOrgId IN (
                            SELECT CTE.Id FROM CTE
                        )"
                    + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                                    s.Code LIKE N'%{request.Keyword}%' OR 
                                    s.Name LIKE N'%{request.Keyword}%') " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<StaffListDto>(pagingSql);
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

            return new PagingResult<StaffListDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
        }

        private async Task<PagingResult<StaffListDto>> GetItemsFromUserAsync(StaffGetListByRole request)
        {
            int fromNumber = request.Skip.HasValue ? request.Skip.Value + 1 : 1;
            int toNumber = fromNumber + (request.MaxResult.HasValue ? request.MaxResult.Value : 0) - 1;
            string sortSql = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("s", request.Sort) : "s.Code";


            string pagingSql = $@"
                        IF OBJECT_ID('tempdb..#Staff') IS NOT NULL
                        DROP TABLE #Staff;

                        WITH CTE AS
                        (
                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN UserAssignments  ON SalesOrgs.Id = UserAssignments.SalesOrgId
	                        WHERE UserAssignments.UserId = {Session.UserId}

                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                        )
                        SELECT s.*, ROW_NUMBER() OVER (ORDER BY {sortSql}) AS RowNumber INTO #Staff 
                        FROM Staffs AS s
                        WHERE s.StaffTypeCode = '{request.StaffTypeCode}' 
                        AND s.SalesOrgId IN (
                            SELECT CTE.Id FROM CTE
                        )"
                        + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                            s.Code LIKE N'%{request.Keyword}%' OR 
                            s.Name LIKE N'%{request.Keyword}%') " : "")
                        + @$"SELECT * FROM #Staff
                        WHERE RowNumber >= {fromNumber} "
                        + (request.MaxResult.HasValue ? @$"AND RowNumber <= {toNumber}" : "");

            string countSql = $@"
                     WITH CTE AS
                        (
                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN UserAssignments  ON SalesOrgs.Id = UserAssignments.SalesOrgId
	                        WHERE UserAssignments.UserId = {Session.UserId}

                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                        )
                        SELECT COUNT(*)
                        FROM Staffs AS s
                        WHERE s.StaffTypeCode = '{request.StaffTypeCode}' 
                        AND s.SalesOrgId IN (
                            SELECT CTE.Id FROM CTE
                        )"
                    + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                                    s.Code LIKE N'%{request.Keyword}%' OR 
                                    s.Name LIKE N'%{request.Keyword}%') " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<StaffListDto>(pagingSql);
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

            return new PagingResult<StaffListDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
        }

    }
}
