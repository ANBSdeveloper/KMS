using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentUpsertFinalSettlementDto : EntityDto
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int DecideUserId { get; set; }
    }
}