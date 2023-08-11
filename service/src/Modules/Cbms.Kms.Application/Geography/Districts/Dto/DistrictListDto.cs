using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Geography.Districts;

namespace Cbms.Kms.Application.Geography.Districts.Dto
{
    [AutoMap(typeof(District))]
    public class DistrictListDto : AuditedEntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public int ProvinceId { get; set; }
    }
}