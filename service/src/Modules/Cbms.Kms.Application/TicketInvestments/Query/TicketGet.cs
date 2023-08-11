using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketGet : EntityQuery<TicketDto>
    {
        public TicketGet(int id) : base(id)
        {
        }
    }
}