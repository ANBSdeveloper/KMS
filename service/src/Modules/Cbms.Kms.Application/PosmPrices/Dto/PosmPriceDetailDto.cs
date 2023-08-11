using Cbms.Dto;
using Cbms.Kms.Domain.PosmItems;

namespace Cbms.Kms.Application.PosmPrices.Dto
{
    public class PosmPriceDetailDto : AuditedEntityDto
    {
        public int PosmItemId { get; set; }
        public decimal Price { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public PosmUnitType UnitType { get; set; }
        public PosmCalcType CalcType { get; set; }
    }
}