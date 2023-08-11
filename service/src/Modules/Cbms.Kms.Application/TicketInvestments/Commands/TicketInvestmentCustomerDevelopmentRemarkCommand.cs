using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentCustomerDevelopmentRemarkCommand : CommandBase
    {
        public TicketInvestmentRemarkDto Data { get; set; }
        public string HandleType { get; set; }

        public TicketInvestmentCustomerDevelopmentRemarkCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
