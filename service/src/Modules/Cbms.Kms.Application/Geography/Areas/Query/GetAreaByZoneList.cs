using Cbms.Kms.Application.Geography.Areas.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Geography.Areas.Query
{
    public class GetAreaByZoneList : EntityPagingResultQuery<AreaDto>
    {
        public int ZoneId { get; set; }
    }
}