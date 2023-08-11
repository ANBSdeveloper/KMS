using Cbms.Kms.Application.Geography.Areas.Dto;
using Cbms.Kms.Application.Geography.Areas.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Geography.Areas.QueryHandler
{
    public class GetAreaByZoneNameListHandler : QueryHandlerBase, IRequestHandler<GetAreaByZoneNameList, PagingResult<AreaByZoneNameListDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetAreaByZoneNameListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<AreaByZoneNameListDto>> Handle(GetAreaByZoneNameList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from area in _dbContext.Areas
                        join zone in _dbContext.Zones on area.ZoneId equals zone.Id

                        select new AreaByZoneNameListDto()
                        {
                            CreationTime = area.CreationTime,
                            CreatorUserId = area.CreatorUserId,
                            Name = area.Name,
                            Id = area.Id,
                            LastModificationTime = area.LastModificationTime,
                            LastModifierUserId = area.LastModifierUserId,
                            Code = area.Code,
                            ZoneId = area.ZoneId,
                            ZoneName = zone.Name
                        };

            query = query
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                    x.Name.Contains(keyword) || x.ZoneName.Contains(keyword));

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
            return new PagingResult<AreaByZoneNameListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}
