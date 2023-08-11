using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketRewardItem : AuditedEntity
    {
        public int RewardItemId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal Amount { get; private set; }
        public int TicketInvestmentId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketRewardItemUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;

            }
        }

        public async Task UpsertAsync(TicketRewardItemUpsertAction action)
        {
            RewardItemId = action.RewardItemId;
            Quantity = action.Quantity;
            Price = action.Price;
            Amount = action.Quantity * action.Price;
        }
    }
}