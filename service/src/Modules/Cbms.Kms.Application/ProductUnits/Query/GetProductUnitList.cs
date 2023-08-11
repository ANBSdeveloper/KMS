using Cbms.Kms.Application.ProductUnits.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductUnits.Query
{
    public class GetProductUnitList : EntityPagingResultQuery<ProductUnitDto>
    {
        public int ProductId { get; set; }
    }
}