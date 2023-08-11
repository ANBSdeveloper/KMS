using Cbms.Kms.Application.Geography.Provinces.Dto;
using Cbms.Kms.Application.Geography.Provinces.Query;
using Cbms.Kms.Domain.Geography.Provinces;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.TenantServers.QueryHandlers
{
    public class GetProvinceListHandler : EntityPagedQueryHandler<GetProvinceList, int, Province, ProvinceDto>
    {
        public GetProvinceListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<Province> Filter(IQueryable<Province> query, GetProvinceList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
    }
}