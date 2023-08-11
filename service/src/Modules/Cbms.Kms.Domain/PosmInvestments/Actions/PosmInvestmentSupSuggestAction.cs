using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentSupSuggestAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int PosmInvestmentItemId { get; private set; }
        public string Reason { get; private set; }
        public PosmInvestmentSupSuggestAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmInvestmentItemId,
            string reason
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            PosmInvestmentItemId= posmInvestmentItemId;
            Reason = reason;
        }
    }
}