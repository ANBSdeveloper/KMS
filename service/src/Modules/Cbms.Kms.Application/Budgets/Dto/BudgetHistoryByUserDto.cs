namespace Cbms.Kms.Application.Budgets.Dto
{

    public class BudgetHistoryByUserDto
    {
        public int InvestmentType { get; set; }
        public decimal AllocateAmount { get; set; }
        public decimal TempUsedAmount { get; set; }
        public decimal TempRemainAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public int Calls { get; set; }
    }
}