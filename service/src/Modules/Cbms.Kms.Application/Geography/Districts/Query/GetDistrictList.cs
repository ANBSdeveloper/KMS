using Cbms.Kms.Application.Geography.Districts.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Geography.Districts.Query
{
    public class GetDistrictList : EntityPagingResultQuery<DistrictListDto>
    {
        public int? ProvinceId { get; set; }
    }
}