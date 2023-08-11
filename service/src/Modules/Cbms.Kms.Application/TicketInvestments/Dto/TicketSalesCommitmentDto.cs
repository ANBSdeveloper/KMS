using Cbms.Dto;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketSalesCommitmentDto : AuditedEntityDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
    }
}