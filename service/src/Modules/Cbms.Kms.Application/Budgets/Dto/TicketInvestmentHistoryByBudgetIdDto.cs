using System;

namespace Cbms.Kms.Application.Budgets.Dto
{
    public class TicketInvestmentHistoryByBudgetIdDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public decimal InvestmentAmount { get; set; }
        public decimal ActualInvestmentAmount { get; set; }
        public int Status { get; set; }
        public int InvestmentType { get; set; }
    }
}