using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.PosmTypes;

namespace Cbms.Kms.Application.PosmTypes.Dto
{
    [AutoMap(typeof(PosmType))]
    public class PosmTypeDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
