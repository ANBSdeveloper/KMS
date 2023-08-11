using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketMaterial : AuditedEntity
    {
        public int MaterialId { get; private set; }
        public int RegisterQuantity { get; private set; }
        public decimal Price { get; private set; }
        public string Note { get; private set; }
        public decimal Amount { get; private set; }
        public int TicketInvestmentId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketMaterialUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;

            }
        }

        public async Task UpsertAsync(TicketMaterialUpsertAction action)
        {
            MaterialId = action.MaterialId;
            RegisterQuantity = action.RegisterQuantity;
            Price = action.Price;
            Amount = action.RegisterQuantity * action.Price;
        }
    }
}