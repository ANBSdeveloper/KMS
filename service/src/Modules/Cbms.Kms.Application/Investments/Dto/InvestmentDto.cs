namespace Cbms.Kms.Application.Investments.Dto
{
    public class InvestmentDto
    {
        public InvestmentAccumulateDto Ticket { get; set; }
        public InvestmentAccumulateDto Posm { get; set; }
        public InvestmentAccumulateDto Pg { get; set; }
        public InvestmentAccumulateDto GoldHour { get; set; }
        public class InvestmentAccumulateDto
        {
            public int HoldingQuantity { get; set; }
            public int ApprovedQuantity { get; set; }
            public int RequestQuantity { get; set; }
        }
    }
}