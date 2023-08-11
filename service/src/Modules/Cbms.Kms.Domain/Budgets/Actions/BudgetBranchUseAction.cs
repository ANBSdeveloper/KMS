using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Budgets.Actions
{
    public class BudgetBranchUseAction : IEntityAction
    {
        public decimal UseAmount { get; private set; }
        public decimal UseTemporaryAmount { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public BudgetBranchUseAction(IIocResolver iocResolver, ILocalizationSource localizationSource, decimal temporaryAmount, decimal useAmount)
        {
            UseAmount = useAmount;
            UseTemporaryAmount = temporaryAmount;
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
        }
    }
}