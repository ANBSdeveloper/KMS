using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.SubProductClasses;

namespace Cbms.Kms.Application.SubProductClasses.Dto
{
    [AutoMap(typeof(SubProductClass))]
    public class SubProductClassDto : AuditedEntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}