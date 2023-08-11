using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketProgressRewardItem : AuditedEntity
    {
        public int RewardItemId { get; private set; }
        public bool IsReceived { get; private set; }
        public int TicketProgressId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketProgressRewardItemUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(TicketProgressRewardItemUpsertAction action)
        {
            RewardItemId = action.RewardItemId;
            IsReceived = action.IsReceived;
        }
    }
}