using Cbms.Dto;

namespace Cbms.Kms.Application.TicketInvestments
{
    public class TicketInvestmentSummaryDto : EntityDto
    {
        public int TicketQuantity { get; set; }
        public int PrintTicketQuantity { get; set; }
        public int SmsTicketQuantity { get; set; }
        public decimal ActualSalesAmount { get; set; }
        public decimal CommitmentSalesAmount { get; set; }
        public decimal? RemarkOfSales { get; set; }
        public decimal? RemarkOfCompany { get; set; }
        public decimal? RemarkOfCustomerDevelopement { get; set; }
    }
}