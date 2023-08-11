using Cbms.Kms.Application.Geography.Provinces.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Geography.Provinces.Query
{
    public class GetProvince : EntityQuery<ProvinceDto>
    {
        public GetProvince(int id) : base(id)
        {
        }
    }
}