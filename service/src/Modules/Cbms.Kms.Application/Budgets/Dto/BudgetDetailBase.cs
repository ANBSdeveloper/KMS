using System;

namespace Cbms.Kms.Application.Budgets.Dto
{
    public class BudgetDetailBase
    {
        public int BudgetId { get; set; }
        public int CycleId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int InvestmentType { get; set; }
        public int StaffId { get; set; }
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public decimal AllocateAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal TempRemainAmount { get; set; }
        public decimal TempUsedAmount { get; set; }
        public int Point { get; set; }
    }
}