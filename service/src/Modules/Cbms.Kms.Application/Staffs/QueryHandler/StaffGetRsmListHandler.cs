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
    public class StaffGetRsmListHandler : QueryHandlerBase, IRequestHandler<StaffGetRsmList, PagingResult<StaffListDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public StaffGetRsmListHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<StaffListDto>> Handle(StaffGetRsmList request, CancellationToken cancellationToken)
        {

            int fromNumber = request.Skip.HasValue ? request.Skip.Value + 1 : 1;
            int toNumber = fromNumber + (request.MaxResult.HasValue ? request.MaxResult.Value : 0) - 1;
            string sortSql = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("s", request.Sort) : "s.Code";
            var staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);

            string cteSql = "";

            if (staff != null)
            {
                cteSql = @$"
                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
	                        WHERE Id = {staff.SalesOrgId}";
            }
            else
            {
                cteSql = $@"SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN UserAssignments  ON SalesOrgs.Id = UserAssignments.SalesOrgId
	                        WHERE UserAssignments.UserId = {Session.UserId}";
            }
            string pagingSql = $@"    
                        WITH CTE AS
                        (
                            {cteSql}

                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE ON SalesOrgs.Id = CTE.ParentId
                        )
                        SELECT s.*, ROW_NUMBER() OVER (ORDER BY {sortSql}) AS RowNumber 
                        INTO #Staff
                        FROM Staffs AS s
                        WHERE s.SalesOrgId IN (
	                        SELECT DISTINCT CTE.Id
	                        FROM CTE
	                        WHERE TypeId = 1144
                        ) "
                        + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                            s.Code LIKE N'%{request.Keyword}%' OR 
                            s.Name LIKE N'%{request.Keyword}%') " : "")
                        + @$"SELECT * FROM #Staff
                        WHERE RowNumber >= {fromNumber} "
                        + (request.MaxResult.HasValue ? @$"AND RowNumber <= {toNumber}" : "");

            string countSql = $@"
                     WITH CTE AS
                        (
                            {cteSql}

                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE ON SalesOrgs.Id = CTE.ParentId
                        )
                        SELECT COUNT(*)
                        FROM Staffs AS s
                        WHERE s.SalesOrgId IN (
	                        SELECT DISTINCT CTE.Id
	                        FROM CTE
	                        WHERE TypeId = 1144
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
