using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketSalesCommitment : AuditedEntity
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public decimal Amount { get; private set; }
        public int TicketInvestmentId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch(action)
            {
                case TicketSalesCommitmentUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                    
            }
        }

        public async Task UpsertAsync(TicketSalesCommitmentUpsertAction action)
        {
            Year = action.Year;
            Month = action.Month;
            Amount = action.Amount;
        }
    }
}