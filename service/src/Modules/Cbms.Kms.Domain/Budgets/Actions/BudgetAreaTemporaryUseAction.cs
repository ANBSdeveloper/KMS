using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Budgets.Actions
{
    public class BudgetAreaTemporaryUseAction : IEntityAction
    {
        public decimal UseAmount { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public BudgetAreaTemporaryUseAction(IIocResolver iocResolver, ILocalizationSource localizationSource, decimal useAmount)
        {
            UseAmount = useAmount;
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
        }
    }
}