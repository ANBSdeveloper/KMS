using Cbms.Kms.Application.Brands.Dto;
using Cbms.Kms.Application.Brands.Query;
using Cbms.Kms.Domain.Brands;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.Brands.QueryHandlers
{
    public class GetBrandListHandler : EntityPagedQueryHandler<GetBrandList, int, Brand, BrandDto>
    {
        public GetBrandListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<Brand> Filter(IQueryable<Brand> query, GetBrandList request)
        {
            var keyword = request.Keyword;
            return query
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
    }
}