using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentUpsertConsumerRewardCommand : UpsertEntityCommand<TicketInvestmentUpsertConsumerRewardDto, TicketConsumerRewardDto>
    {
        public TicketInvestmentUpsertConsumerRewardCommand(TicketInvestmentUpsertConsumerRewardDto data, string handleType) : base(data, handleType)
        {
        }

        public TicketInvestmentUpsertConsumerRewardCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }

        public TicketInvestmentUpsertConsumerRewardCommand WithRewardItemId(int rewardItemId)
        {
            Data.RewardItemId = rewardItemId;
            return this;
        }
    }
}