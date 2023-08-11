using Cbms.Kms.Application.PosmPrices.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmPrices.Query
{
    public class PosmPriceHeaderGet : EntityQuery<PosmPriceHeaderDto>
    {
        public PosmPriceHeaderGet(int id) : base(id)
        {
        }
    }
}