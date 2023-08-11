using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmentTrackingGet : EntityQuery<TicketInvestmentTrackingDto>
    {
        public TicketInvestmentTrackingGet(int id) : base(id)
        {
        }
    }
}