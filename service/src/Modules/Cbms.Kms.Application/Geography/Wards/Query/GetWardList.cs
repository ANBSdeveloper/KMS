using Cbms.Kms.Application.Geography.Wards.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Geography.Wards.Query
{
    public class GetWardList : EntityPagingResultQuery<WardDto>
    {
        public int? DistrictId { get; set; }
    }
}
