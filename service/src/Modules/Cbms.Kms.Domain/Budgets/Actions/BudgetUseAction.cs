using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Budgets.Actions
{
    public class BudgetUseAction : IEntityAction
    {
        public BudgetLevelType LevelType { get; private set; }
        public int ObjectId { get; private set; }
        public decimal UseAmount { get; private set; }
        public decimal TemporaryAmount { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }

        public BudgetUseAction(IIocResolver iocResolver, ILocalizationSource localizationSource, BudgetLevelType levelType, int objectId, decimal temporaryAmount, decimal useAmount)
        {
            LevelType = levelType;
            ObjectId = objectId;
            UseAmount = useAmount;
            TemporaryAmount = temporaryAmount;
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
        }
    }
}