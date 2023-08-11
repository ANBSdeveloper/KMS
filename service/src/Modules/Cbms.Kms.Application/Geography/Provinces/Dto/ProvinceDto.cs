using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Geography.Provinces;

namespace Cbms.Kms.Application.Geography.Provinces.Dto
{
    [AutoMap(typeof(Province))]
    public class ProvinceDto : AuditedEntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}