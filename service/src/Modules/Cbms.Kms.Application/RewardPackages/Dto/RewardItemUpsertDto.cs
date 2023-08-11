using Cbms.Dto;

namespace Cbms.Kms.Application.RewardPackages.Dto
{
    public class RewardItemUpsertDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DocumentLink { get; set; }
        public int? ProductUnitId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? ProductId { get; set; }
    }
}