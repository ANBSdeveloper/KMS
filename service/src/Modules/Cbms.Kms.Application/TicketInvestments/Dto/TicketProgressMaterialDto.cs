using Cbms.Dto;

namespace Cbms.Kms.Application.TicketInvestments
{
    public class TicketProgressMaterialDto : AuditedEntityDto
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public bool IsReceived { get; set; }
        public bool IsSentDesign { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }
        public decimal Price { get; set; }
        public bool IsDesign { get; set; }
        public int RegisterQuantity { get; set; }
        public decimal Amount { get; set; }
    }
}