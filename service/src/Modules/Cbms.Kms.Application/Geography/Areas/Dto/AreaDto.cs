using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Areas;

namespace Cbms.Kms.Application.Geography.Areas.Dto
{
    [AutoMap(typeof(Area))]
    public class AreaDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int ZoneId { get; set; }
        public int SalesOrgId { get; set; }
    }
}