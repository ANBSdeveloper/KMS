using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentCompanyRemarkCommand : CommandBase
    {
        public TicketInvestmentRemarkDto Data { get; set; }
        public string HandleType { get; set; }

        public TicketInvestmentCompanyRemarkCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
