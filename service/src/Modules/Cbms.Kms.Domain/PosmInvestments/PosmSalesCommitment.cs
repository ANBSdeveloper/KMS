using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.PosmInvestments
{
    public class PosmSalesCommitment : AuditedEntity
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public decimal Amount { get; private set; }
        public int PosmInvestmentId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch(action)
            {
                case PosmSalesCommitmentUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                    
            }
        }

        public async Task UpsertAsync(PosmSalesCommitmentUpsertAction action)
        {
            Year = action.Year;
            Month = action.Month;
            Amount = action.Amount;
        }
    }
}