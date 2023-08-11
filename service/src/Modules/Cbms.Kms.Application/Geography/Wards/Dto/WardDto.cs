using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Geography.Wards;

namespace Cbms.Kms.Application.Geography.Wards.Dto
{
    [AutoMap(typeof(Ward))]
    public class WardDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
