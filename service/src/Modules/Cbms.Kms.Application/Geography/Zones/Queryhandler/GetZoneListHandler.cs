using Cbms.Kms.Application.Geography.Zones.Dto;
using Cbms.Kms.Application.Geography.Zones.Query;
using Cbms.Kms.Domain.Zones;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.Geography.Zones.Queryhandler
{
    public class GetZoneListHandler : EntityPagedQueryHandler<GetZoneList, int, Zone, ZoneDto>
    {
        public GetZoneListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<Zone> Filter(IQueryable<Zone> query, GetZoneList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
    }
}