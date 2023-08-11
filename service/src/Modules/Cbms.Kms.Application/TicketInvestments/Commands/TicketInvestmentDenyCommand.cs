using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentDenyCommand : CommandBase
    {
        public int Id { get; private set; }
        public TicketInvestmentDenyCommand(int id)
        {
            Id = id;
        }
    }
}
