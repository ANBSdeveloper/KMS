using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.TicketInvestments
{
    public class TicketFinalSettlementDto : AuditedEntityDto
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int? DecideUserId { get; set; }
        public string DecideUserName { get; set; }
        public int? UpdateUserId { get; set; }
        public string UpdateUserName { get; set; }
    }
}