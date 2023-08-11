using Cbms.Kms.Application.ProductClasses.Dto;
using Cbms.Kms.Application.ProductClasses.Query;
using Cbms.Kms.Domain.ProductClasses;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.TenantServers.QueryHandlers
{
    public class GetProductClassListHandler : EntityPagedQueryHandler<GetProductClassList, int, ProductClass, ProductClassDto>
    {
        public GetProductClassListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<ProductClass> Filter(IQueryable<ProductClass> query, GetProductClassList request)
        {
            var keyword = request.Keyword;
            return query
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
    }
}