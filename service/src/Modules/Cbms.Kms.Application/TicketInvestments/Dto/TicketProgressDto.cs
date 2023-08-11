using Cbms.Dto;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketProgressDto : AuditedEntityDto
    {
        public DateTime UpdateTime { get; set; }
        public string DocumentPhoto1 { get; set; }
        public string DocumentPhoto2 { get; set; }
        public string DocumentPhoto3 { get; set; }
        public string DocumentPhoto4 { get; set; }
        public string DocumentPhoto5 { get; set; }
        public string Note { get; set; }
        public List<TicketProgressMaterialDto> Materials { get; set; }
        public List<TicketProgressRewardItemDto> RewardItems { get; set; }
        public int UpdateUserId { get; set; }
        public string UpdateUserName { get; set; }
    }
}
