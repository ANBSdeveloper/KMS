using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.PosmPrices.Actions
{
    public class PosmPriceDetailUpsertAction : IEntityAction
    {
        public int? Id { get; private set; }
        public int PosmItemId { get; private set; }
        public decimal Price { get; private set; }

        public PosmPriceDetailUpsertAction(
            int? id,
            int posmItemId,
            decimal price)
        {
            Id = id;
            PosmItemId = posmItemId;
            Price = price;
        }
    }
}