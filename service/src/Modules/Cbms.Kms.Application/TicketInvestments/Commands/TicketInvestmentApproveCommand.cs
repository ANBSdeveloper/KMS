using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentApproveCommand : CommandBase
    {
        public int Id { get; private set; }
        public TicketInvestmentApproveCommand(int id)
        {
            Id = id;
        }
    }
}
