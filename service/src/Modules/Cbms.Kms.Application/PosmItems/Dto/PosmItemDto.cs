using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.PosmItems;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmItems.Dto
{
    [AutoMap(typeof(PosmItem))]
    public class PosmItemDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int PosmClassId { get; set; }
        public int? PosmTypeId { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
        public PosmUnitType UnitType { get; set; }
        public PosmCalcType CalcType { get; set; }
        public List<PosmCatalogDto> Catalogs { get; set; }
    }
}
