using Cbms.Dto;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketMaterialDto : AuditedEntityDto
    {
        public int MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public int RegisterQuantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public decimal Amount { get; set; }
        public bool IsDesign { get;  set; }
    }
}