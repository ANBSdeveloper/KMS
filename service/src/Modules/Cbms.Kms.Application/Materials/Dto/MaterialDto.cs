using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Materials;

namespace Cbms.Kms.Application.Materials.Dto
{
    [AutoMap(typeof(Material))]
    public class MaterialDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int MaterialTypeId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public bool IsActive { get; set; }
        public bool IsDesign { get; set; }
    }
}
