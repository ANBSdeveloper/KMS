using Cbms.Dto;

namespace Cbms.Kms.Application.Orders.Dto
{
    public class OrderDetailDto: AuditedEntityDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string QrCode { get; set; }
        public string SpoonCode { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineAmount { get; set; }
        public string Api { get; set; }
        public decimal Points { get; set; }
        public decimal AvailablePoints { get; set; }
        public decimal UsedPoints { get; set; }
        public bool UsedForTicket { get; set; }
    }
}