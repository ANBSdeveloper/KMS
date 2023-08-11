using Cbms.Dto;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketRewardItemDto : AuditedEntityDto
    {
        public int RewardItemId { get; set; }
        public string RewardItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string DocumentLink { get; set; }
        public string RewardItemCode { get; set; }
    }
}