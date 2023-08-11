using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketInvestmentHistory : Entity<int>, ICreationAudited
    {
        public string Data { get; set; }
        public TicketInvestmentStatus Status { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public int TicketInvestmentId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketInvestmentHistoryUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(TicketInvestmentHistoryUpsertAction action)
        {
            Data = action.Data;
            Status = action.Status;
        }
    }
}