using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Geography.Districts;

namespace Cbms.Kms.Application.Geography.Districts.Dto
{
    [AutoMap(typeof(District))]
    public class DistrictDto : AuditedEntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public int ProvinceId { get; set; }
    }
}