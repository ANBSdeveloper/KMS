using Cbms.Kms.Application.Geography.Districts.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Geography.Districts.Query
{
    public class GetDistrict : EntityQuery<DistrictDto>
    {
        public GetDistrict(int id) : base(id)
        {
        }
    }
}