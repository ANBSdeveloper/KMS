using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmentActiveGet : QueryBase, IRequest<TicketInvestmentDto>
    {
        public TicketInvestmentActiveGet() : base()
        {
        }
    }
}