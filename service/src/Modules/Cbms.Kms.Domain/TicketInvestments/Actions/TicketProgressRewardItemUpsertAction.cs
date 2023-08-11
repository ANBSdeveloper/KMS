using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketProgressRewardItemUpsertAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public bool IsReceived { get; private set; }
        public int RewardItemId { get; private set; }
        public TicketProgressRewardItemUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int rewardItemId,
            bool isReceived
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            IsReceived = isReceived;
            RewardItemId = rewardItemId;
        }

    }
}