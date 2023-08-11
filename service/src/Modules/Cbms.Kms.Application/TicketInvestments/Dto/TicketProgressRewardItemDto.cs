using Cbms.Dto;

namespace Cbms.Kms.Application.TicketInvestments
{
    public class TicketProgressRewardItemDto : AuditedEntityDto
    {
        public int RewardItemId { get;  set; }
        public string RewardItemCode { get; set; }
        public string RewardItemName { get; set; }
        public bool IsReceived { get;  set; }
        public string DocumentLink { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}