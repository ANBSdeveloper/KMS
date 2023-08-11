using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.CustomerLocations;

namespace Cbms.Kms.Application.CustomerLocations.Dto
{
    [AutoMap(typeof(CustomerLocation))]
    public class CustomerLocationDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
