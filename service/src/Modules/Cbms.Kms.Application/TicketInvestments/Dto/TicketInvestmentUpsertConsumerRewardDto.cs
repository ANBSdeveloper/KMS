using Cbms.Dto;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentUpsertConsumerRewardDto : AuditedEntityDto
    {
        public int RewardItemId { get;  set; }
        public int Quantity { get;  set; }
        public string Photo1 { get;  set; }
        public string Photo2 { get;  set; }
        public string Photo3 { get;  set; }
        public string Photo4 { get;  set; }
        public string Photo5 { get;  set; }
        public CrudListDto<ConsumerRewardDetail> DetailChanges { get;  set; }

        public class ConsumerRewardDetail
        {
            public int Id { get;  set; }
            public int TicketId { get;  set; }
            public string Note { get;  set; }
        }
    }
}
