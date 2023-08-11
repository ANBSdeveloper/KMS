using Cbms.Kms.Application.CustomerLocations.Dto;
using Cbms.Kms.Application.CustomerLocations.Query;
using Cbms.Kms.Domain.CustomerLocations;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.CustomerLocations.QueryHandler
{
    public class CustomerLocationGetListHandler : EntityPagedQueryHandler<CustomerLocationGetList, int, CustomerLocation, CustomerLocationDto>
    {
        public CustomerLocationGetListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<CustomerLocation> Filter(IQueryable<CustomerLocation> query, CustomerLocationGetList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword))
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive);
        }
    }
}