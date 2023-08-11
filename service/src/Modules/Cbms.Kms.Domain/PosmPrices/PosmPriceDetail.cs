using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.PosmPrices.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.PosmPrices
{
    public class PosmPriceDetail : AuditedEntity
    {
        public int PosmPriceHeaderId { get; private set; }
        public int PosmItemId { get; private set; }
        public decimal Price { get; private set; }

        private PosmPriceDetail()
        {
        }

        public static PosmPriceDetail Create()
        {
            return new PosmPriceDetail();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmPriceDetailUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(PosmPriceDetailUpsertAction action)
        {
            PosmItemId = action.PosmItemId;
            Price = action.Price;
        }
    }
}