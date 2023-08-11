using Cbms.Kms.Application.Branches.Dto;
using Cbms.Kms.Application.Branches.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Branches.QueryHandler
{
    public class GetBranchListHandler : QueryHandlerBase, IRequestHandler<GetBranchList, PagingResult<BranchListItemDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetBranchListHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PagingResult<BranchListItemDto>> Handle(GetBranchList request, CancellationToken cancellationToken)
        {
          
            string query = $@"select a.Id, a.SalesOrgId
                , a.Name
                , a.Code
                , a.CreationTime
                , a.CreatorUserId
                , a.IsActive
                , a.LastModificationTime
                , a.LastModifierUserId
                , a.AreaId
                , AreaName = b.Name
                , a.ZoneId
                , ZoneName = c.Name
                , a.ChannelId
                , ChannelName = d.Name
                , ProvinccId = province.ProvinceId
                , ProvinceName = province.ProvinceName
                from Branches as a
                LEFT JOIN Areas as b ON a.AreaId = b.Id
                LEFT JOIN Zones as c ON a.ZoneId = c.Id
                LEFT JOIN Channels as d ON a.ChannelId = d.Id
                outer apply (
	                SELECT TOP 1 ProvinceId = p.Id, ProvinceName = p.Name
	                FROM Customers AS k
	                INNER JOIN Provinces AS p ON k.ProvinceId = p.Id
	                WHERE k.BranchId = a.Id AND k.ProvinceId IS NOT NULL
                ) AS province
                WHERE (a.Code LIKE N'%{request.Keyword}%' OR a.Name LIKE N'%{request.Keyword}%' )"
                + (request.IsActive.HasValue ? $@"AND a.IsActive = {(request.IsActive.Value ? "1" : "0")} " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<BranchListItemDto>(query);

            items = items.ToList().SortFromString(request.Sort);

            int totalCount = items.Count();
            if (request.Skip.HasValue)
            {
                items = items.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                items = items.Take(request.MaxResult.Value);
            }
            return new PagingResult<BranchListItemDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
        }
    }
}