using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Budgets.Actions
{
    public class BudgetTemporaryUseAction : IEntityAction
    {
        public BudgetLevelType LevelType { get; private set; }
        public int ObjectId { get; private set; }
        public decimal UseAmount { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }

        public BudgetTemporaryUseAction(IIocResolver iocResolver, ILocalizationSource localizationSource, BudgetLevelType levelType, int objectId, decimal useAmount)
        {
            LevelType = levelType;
            ObjectId = objectId;
            UseAmount = useAmount;
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
        }
    }
}