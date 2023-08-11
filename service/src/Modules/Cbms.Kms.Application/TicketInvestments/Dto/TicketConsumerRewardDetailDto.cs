using Cbms.Dto;

namespace Cbms.Kms.Application.TicketInvestments
{
    public class TicketConsumerRewardDetailDto : AuditedEntityDto
    {
        public int TicketId { get; set; }
        public string TicketCode { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerPhone { get; set; }
        public string Note { get; set; }
    }
}