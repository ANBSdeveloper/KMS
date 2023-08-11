using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.TicketInvestments;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments
{
    [AutoMap(typeof(TicketConsumerReward))]
    public class TicketConsumerRewardDto : AuditedEntityDto
    {
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }
        public int RewardItemId { get; set; }
        public string RewardItemCode { get; set; }
        public string RewardItemName { get; set; }
        public int Quantity { get; set; }
        public int RewardQuantity { get; set; }
        [IgnoreMap]
        public List<TicketConsumerRewardDetailDto> Details { get; set; }
    }
}