using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.MaterialTypes;

namespace Cbms.Kms.Application.MaterialTypes.Dto
{
    [AutoMap(typeof(MaterialType))]
    public class MaterialTypeDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
