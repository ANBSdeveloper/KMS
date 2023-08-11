using Cbms.Kms.Application.Geography.Wards.Dto;
using Cbms.Kms.Application.Geography.Wards.Query;
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
    public class GetWardNameListHandler : QueryHandlerBase, IRequestHandler<GetWardNameList, PagingResult<WardNameListDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetWardNameListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<WardNameListDto>> Handle(GetWardNameList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from ward in _dbContext.Wards
                        join district in _dbContext.Districts on ward.DistrictId equals district.Id
                        join province in _dbContext.Provinces on district.ProvinceId equals province.Id

                        select new WardNameListDto()
                        {
                            CreationTime = ward.CreationTime,
                            CreatorUserId = ward.CreatorUserId,
                            Name = ward.Name,
                            Id = ward.Id,
                            LastModificationTime = ward.LastModificationTime,
                            LastModifierUserId = ward.LastModifierUserId,
                            Code = ward.Code,
                            Province = province.Name,
                            District = district.Name
                        };

            query = query
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                    x.Name.Contains(keyword) || x.Province.Contains(keyword) || x.District.Contains(keyword));

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
            return new PagingResult<WardNameListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}
