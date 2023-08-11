using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketConsumerRewardGet : EntityQuery<TicketConsumerRewardDto>
    {
        public TicketConsumerRewardGet(int id) : base(id)
        {
        }
    }
}