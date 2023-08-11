using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmentGet : EntityQuery<TicketInvestmentDto>
    {
        public TicketInvestmentGet(int id) : base(id)
        {
        }
    }
}