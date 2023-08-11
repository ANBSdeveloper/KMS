using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmentSummaryGet : EntityQuery<TicketInvestmentSummaryDto>
    {
        public TicketInvestmentSummaryGet(int id) : base(id)
        {
        }
    }
}