using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentRegisterCommand : CommandBase<TicketInvestmentDto>
    {
        public TicketInvestmentRegisterDto Data { get; set; }
    }
}
