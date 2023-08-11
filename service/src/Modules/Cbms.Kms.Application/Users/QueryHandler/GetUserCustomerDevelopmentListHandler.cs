using Cbms.Application.Users.Dto;
using Cbms.Application.Users.Query;
using Cbms.Kms.Domain;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Application.Users.QueryHandlers
{
    public class GetUserCustomerDevelopmentListHandler : QueryHandlerBase, IRequestHandler<GetUserCustomerDevelopmentList, PagingResult<UserListItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserCustomerDevelopmentListHandler(IRequestSupplement supplement, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PagingResult<UserListItemDto>> Handle(GetUserCustomerDevelopmentList request, CancellationToken cancellationToken)
        {
            int fromNumber = request.Skip.HasValue ? request.Skip.Value + 1 : 1;
            int toNumber = fromNumber + (request.MaxResult.HasValue ? request.MaxResult.Value : 0) - 1;
            var sqlSort = !string.IsNullOrEmpty(request.Sort) ? QueryHelper.SqlSortFromString("o", request.Sort) : "o.UserName";
            var pagingSql = @$"
                        IF OBJECT_ID('tempdb..#TempUser') IS NOT NULL
                                DROP TABLE #TempUser;

                        SELECT
                            o.*,
                            ROW_NUMBER() OVER (ORDER BY {sqlSort}) AS RowNumber
                        INTO #TempUser
                        FROM (
                            SELECT
                                c.*,
	                            a.RoleId,
	                            a.RoleName                                
                            FROM Users AS c
                            OUTER APPLY (
	                            SELECT TOP 1 RoleId = r.Id, RoleName = DisplayName
	                            FROM Roles AS r
	                            INNER JOIN UserRoles AS ur ON r.Id = ur.RoleId
	                            WHERE ur.UserId = c.Id
                            ) AS a
                            WHERE (
                                c.UserName LIKE N'%{request.Keyword}%' OR
                                c.Name LIKE N'%{request.Keyword}%' OR
                                c.PhoneNumber LIKE N'%{request.Keyword}%' OR
                                c.EmailAddress LIKE N'%{request.Keyword}%'
                            )
                        AND EXISTS(
                                SELECT TOP 1 *
                                FROM Roles AS r
	                            INNER JOIN UserRoles AS ur ON r.Id = ur.RoleId
	                            WHERE ur.UserId = c.Id AND r.RoleName IN @RoleName
                            )
                        ) AS o "
                        + @$"SELECT * FROM #TempUser "
                        + @$" WHERE RowNumber >= {fromNumber} "
                        + (request.MaxResult.HasValue ? @$"AND RowNumber <= {toNumber}" : "");

            string countSql = $@"
                    SELECT
                        COUNT(*)
                    FROM Users AS c
                    OUTER APPLY (
	                    SELECT TOP 1 RoleId = r.Id, RoleName = DisplayName
	                    FROM Roles AS r
	                    INNER JOIN UserRoles AS ur ON r.Id = ur.RoleId
	                    WHERE ur.UserId = c.Id
                    ) AS a
                    WHERE (
                        c.UserName LIKE N'%{request.Keyword}%' OR
                        c.Name LIKE N'%{request.Keyword}%' OR
                        c.PhoneNumber LIKE N'%{request.Keyword}%' OR
                        c.EmailAddress LIKE N'%{request.Keyword}%'
                    )
                    AND EXISTS(
                            SELECT TOP 1 *
                            FROM Roles AS r
                            INNER JOIN UserRoles AS ur ON r.Id = ur.RoleId
                            WHERE ur.UserId = c.Id AND r.RoleName IN @RoleName
                        )";

            using (var connection = await _sqlConnectionFactory.GetConnectionAsync())
            {
                var items = await connection.QueryAsync<UserListItemDto>(pagingSql, new
                {
                    RoleName = new List<string>() { KmsConsts.CustomerDevelopmentManagerRole, KmsConsts.CustomerDevelopmentLeadRole }
                });
                var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new
                {
                    RoleName = new List<string>() { KmsConsts.CustomerDevelopmentManagerRole, KmsConsts.CustomerDevelopmentLeadRole }
                });

                return new PagingResult<UserListItemDto>()
                {
                    Items = items.ToList(),
                    TotalCount = totalCount
                };
            }
        }
    }
}