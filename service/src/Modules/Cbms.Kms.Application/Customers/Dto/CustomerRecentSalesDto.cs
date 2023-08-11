using System.Collections.Generic;

namespace Cbms.Kms.Application.Customers.Dto
{
    public class CustomerRecentSalesDto
    {
        public List<MonthData> MonthData { get; set; }
        public List<YearData> YearData { get; set; }
    }

    public class MonthData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
    }

    public class YearData
    {
        public string Measure { get; set; }
        public decimal Amount { get; set; }
    }
}