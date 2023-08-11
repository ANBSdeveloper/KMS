using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.PosmInvestments
{
    public class PosmInvestmentItemHistory : Entity<int>, ICreationAudited
    {
        public string Data { get; set; }
        public PosmInvestmentItemStatus Status { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public int PosmInvestmentItemId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmInvestmentItemHistoryUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(PosmInvestmentItemHistoryUpsertAction action)
        {
            Data = action.Data;
            Status = action.Status;
        }
    }
}