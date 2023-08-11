using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.PosmItems;

namespace Cbms.Kms.Application.PosmItems.Dto
{
    [AutoMap(typeof(PosmCatalog))]
    public class PosmCatalogDto : AuditedEntityDto
    {
        public int PosmItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
