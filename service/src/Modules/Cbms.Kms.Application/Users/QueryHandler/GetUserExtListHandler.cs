using Cbms.Application.Users.Dto;
using Cbms.Application.Users.Query;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Application.Users.QueryHandlers
{
    public class GetUserExtListHandler : QueryHandlerBase, IRequestHandler<GetUserExtList, PagingResult<UserListItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserExtListHandler(IRequestSupplement supplement, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PagingResult<UserListItemDto>> Handle(GetUserExtList request, CancellationToken cancellationToken)
        {
            int fromNumber = request.Skip.HasValue ? request.Skip.Value + 1 : 1;
            int toNumber = fromNumber + (request.MaxResult.HasValue ? request.MaxResult.Value : 0) - 1;
            var sqlSort = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("c", request.Sort) : "c.UserName";
            var pagingSql = @$"
                        IF OBJECT_ID('tempdb..#TempUser') IS NOT NULL
                                DROP TABLE #TempUser;

                        
                        SELECT 
                            c.*,
                            ROW_NUMBER() OVER (ORDER BY {sqlSort}) AS RowNumber
                        INTO #TempUser
                        FROM (
                            SELECT
                                c.*,
	                            a.RoleId,
	                            a.RoleName                                
                            FROM Users AS c
                            OUTER APPLY (
	                            SELECT TOP 1 RoleId = r.Id, RoleName = DisplayName, RoleCode = RoleName
	                            FROM Roles AS r
	                            INNER JOIN UserRoles AS ur ON r.Id = ur.RoleId
	                            WHERE ur.UserId = c.Id
                            ) AS a
                            WHERE (
                                c.UserName LIKE N'%{request.Keyword}%' OR
                                c.Name LIKE N'%{request.Keyword}%' OR
                                c.PhoneNumber LIKE N'%{request.Keyword}%' OR
                                c.EmailAddress LIKE N'%{request.Keyword}%'
                            ) "
                        + (request.RoleId.HasValue ? $" AND a.RoleId = {request.RoleId} " : " ")
                        + (request.IsActive.HasValue ? $" AND c.IsActive = @IsActive " : " ")
                        + (!string.IsNullOrEmpty(request.RoleName) ? $" AND a.RoleCode = '{request.RoleName}' " : " ")
                        + @$") AS c "
                        + @$"SELECT * FROM #TempUser
                         WHERE RowNumber >= {fromNumber} "
                        + (request.MaxResult.HasValue ? @$"AND RowNumber <= {toNumber}" : "");

            string countSql = $@"
                    SELECT
                        COUNT(*)
                    FROM Users AS c
                    OUTER APPLY (
	                    SELECT TOP 1 RoleId = r.Id, RoleName = DisplayName, RoleCode = RoleName
	                    FROM Roles AS r
	                    INNER JOIN UserRoles AS ur ON r.Id = ur.RoleId
	                    WHERE ur.UserId = c.Id
                    ) AS a
                    WHERE (
                        c.UserName LIKE N'%{request.Keyword}%' OR
                        c.Name LIKE N'%{request.Keyword}%' OR
                        c.PhoneNumber LIKE N'%{request.Keyword}%' OR
                        c.EmailAddress LIKE N'%{request.Keyword}%'
                    ) "
                + (request.RoleId.HasValue ? $" AND a.RoleId = {request.RoleId} " : " ")
                + (request.IsActive.HasValue ? $" AND c.IsActive = @IsActive " : " ")
                + (!string.IsNullOrEmpty(request.RoleName) ? $" AND a.RoleCode = '{request.RoleName}' " : " ");

            using (var connection = await _sqlConnectionFactory.GetConnectionAsync())
            {
                var items = await connection.QueryAsync<UserListItemDto>(pagingSql, new { request.IsActive });
                var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new { request.IsActive });

                return new PagingResult<UserListItemDto>()
                {
                    Items = items.ToList(),
                    TotalCount = totalCount
                };
            }
        }
    }
}