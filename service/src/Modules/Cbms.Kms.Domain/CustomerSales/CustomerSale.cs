using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.CustomerSales.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.CustomerSales
{
    public class CustomerSale : AuditedAggregateRoot
    {
        public int CustomerId { get; private set; }
        public string Year { get; private set; }
        public string Month { get; private set; }
        public string YearMonth { get; private set; }
        public decimal Amount { get; private set; }

        public CustomerSale()
        {
        }

        public static CustomerSale Create()
        {
            return new CustomerSale();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case CustomerSaleUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(CustomerSaleUpsertAction action)
        {
            CustomerId = action.CustomerId;
            Year = action.Year;
            Month = action.Month;
            YearMonth = action.YearMonth;
            Amount = action.Amount;
        }
    }
}