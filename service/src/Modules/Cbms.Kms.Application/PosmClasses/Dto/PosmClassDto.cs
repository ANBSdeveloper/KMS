using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.PosmClasses;

namespace Cbms.Kms.Application.PosmClasses.Dto
{
    [AutoMap(typeof(PosmClass))]
    public class PosmClassDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IncludeInfo { get; set; }
        public bool IsActive { get; set; }
    }
}
