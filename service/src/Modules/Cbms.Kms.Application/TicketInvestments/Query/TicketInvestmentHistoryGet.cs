using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmentHistoryGet : EntityQuery<List<TicketInvestmentHistoryDto>>
    {
        public TicketInvestmentHistoryGet(int id) : base(id)
        {
        }
    }
}