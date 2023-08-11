using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentUpdateCommand : CommandBase
    {
        public TicketInvestmentUpdateDto Data { get; set; }
    }
}
