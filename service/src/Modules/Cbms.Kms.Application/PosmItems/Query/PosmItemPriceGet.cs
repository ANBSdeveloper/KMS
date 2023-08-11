using MediatR;

namespace Cbms.Kms.Application.PosmItems.Query
{
    public class PosmItemPriceGet : IRequest<decimal>
    {
        public int PosmItemId { get; private set; }
        public PosmItemPriceGet(int id) { PosmItemId = id; }
    }
}