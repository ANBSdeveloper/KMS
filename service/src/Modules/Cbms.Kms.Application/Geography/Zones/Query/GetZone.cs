using Cbms.Kms.Application.Geography.Zones.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Geography.Zones.Query
{
    public class GetZone : EntityQuery<ZoneDto>
    {
        public GetZone(int id) : base(id)
        {
        }
    }
}