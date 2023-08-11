using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketGetList : EntityPagingResultQuery<TicketDto>
    {
        public TicketGetList(int id)
        {
            TicketInvestmentId = id;
        }
        public int TicketInvestmentId { get; private set; }
    }
}