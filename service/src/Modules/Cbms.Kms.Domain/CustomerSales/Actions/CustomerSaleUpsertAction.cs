using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.CustomerSales.Actions
{
    public class CustomerSaleUpsertAction : IEntityAction
    {
        public int CustomerId { get; private set; }
        public string Year { get; private set; }
        public string Month { get; private set; }
        public string YearMonth { get; private set; }
        public decimal Amount { get; private set; }

        public CustomerSaleUpsertAction(int customerId, string year, string month, string yearMonth, decimal amount)
        {
            CustomerId = customerId;
            Year = year;
            Month = month;
            YearMonth = yearMonth;
            Amount = amount;
        }
    }
}