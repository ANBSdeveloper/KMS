using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketRewardItemUpsertAction : IEntityAction
    {
        public int RewardItemId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public TicketRewardItemUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int rewardItemId,
            int quantity,
            decimal price
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            RewardItemId = rewardItemId;
            Quantity = quantity;
            Price = price;
        }
    }
}