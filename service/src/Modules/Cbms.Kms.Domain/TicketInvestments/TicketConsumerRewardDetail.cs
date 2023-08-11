using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketConsumerRewardDetail : AuditedEntity
    {
        public int TicketId { get; private set; }
        public string Note { get; private set; }
        public int TicketConsumerRewardId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketConsumerRewardDetailUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(TicketConsumerRewardDetailUpsertAction action)
        {
            TicketId = action.TicketId;
            Note = action.Note ?? "";
        }
    }
}