using Cbms.Kms.Application.Geography.Districts.Dto;
using Cbms.Kms.Application.Geography.Districts.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Geography.Districts.QueryHandlers
{
    public class GetDistrictListHandler : QueryHandlerBase, IRequestHandler<GetDistrictList, PagingResult<DistrictListDto>>
    { 
        private readonly AppDbContext _dbContext;
        public GetDistrictListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<DistrictListDto>> Handle(GetDistrictList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from district in _dbContext.Districts
                        join province in _dbContext.Provinces on district.ProvinceId equals province.Id
                        select new DistrictListDto()
                        {
                            Code = district.Code,
                            Name = district.Name,
                            ProvinceCode = province.Code,
                            ProvinceName = province.Name,
                            Id = district.Id,
                            ProvinceId = district.ProvinceId
                        };

            query = query
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                    x.Name.Contains(keyword))
                .WhereIf(request.ProvinceId.HasValue, x => x.ProvinceId == (int)request.ProvinceId);

            int totalCount = query.Count();
            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<DistrictListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}