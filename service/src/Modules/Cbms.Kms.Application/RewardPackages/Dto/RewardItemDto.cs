using Cbms.Dto;

namespace Cbms.Kms.Application.RewardPackages.Dto
{
    public class RewardItemDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DocumentLink { get; set; }
        public int? ProductUnitId { get; set; }
        public string ProductUnitCode { get; set; }
        public string ProductUnitName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}