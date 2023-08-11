using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Zones;

namespace Cbms.Kms.Application.Geography.Zones.Dto
{
    [AutoMap(typeof(Zone))]
    public class ZoneDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int SalesOrgId { get; set; }
    }
}