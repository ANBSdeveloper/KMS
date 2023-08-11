using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Cbms.Dto;
using Cbms.Kms.Domain.Brands;

namespace Cbms.Kms.Application.Brands.Dto
{
    [AutoMap(typeof(Brand))]
    public class BrandDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}