using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentFinalAcceptanceAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int PosmInvestmentItemId { get; private set; }
        public PosmInvestmentFinalAcceptanceAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmInvestmentItemId
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            PosmInvestmentItemId= posmInvestmentItemId;
        }
    }
}