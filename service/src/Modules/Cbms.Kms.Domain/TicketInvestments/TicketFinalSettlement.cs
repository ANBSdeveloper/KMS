using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketFinalSettlement : AuditedEntity
    {
        public DateTime Date { get; private set; }
        public string Note { get; private set; }
        public int TicketInvestmentId { get; private set; }
        public int DecideUserId { get; private set; }
        public int UpdateUserId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketFinalSettlementUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(TicketFinalSettlementUpsertAction action)
        {
            Date = action.Date.ToLocalTime().Date;
            UpdateUserId = action.UserId;
            DecideUserId = action.DecideUserId;
            Note = action.Note ?? "";
        }
    }
}