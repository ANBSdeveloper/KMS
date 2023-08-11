using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentSalesRemarkCommand : CommandBase
    {
        public TicketInvestmentRemarkDto Data { get; set; }

        public TicketInvestmentSalesRemarkCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
