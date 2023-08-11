using Cbms.Kms.Application.ProductUnits.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductUnits.Query
{
    public class GetProductUnit : EntityQuery<ProductUnitDto>
    {
        public GetProductUnit(int id) : base(id)
        {
        }
    }
}