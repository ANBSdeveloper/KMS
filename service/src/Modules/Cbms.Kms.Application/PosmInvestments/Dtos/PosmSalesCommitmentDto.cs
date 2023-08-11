using Cbms.Dto;

namespace Cbms.Kms.Application.PosmInvestments.Dto
{
    public class PosmSalesCommitmentDto : AuditedEntityDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
    }
}