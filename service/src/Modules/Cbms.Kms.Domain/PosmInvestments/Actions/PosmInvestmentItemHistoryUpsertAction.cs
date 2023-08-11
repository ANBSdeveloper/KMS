using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{
    public class PosmInvestmentItemHistoryUpsertAction : IEntityAction
    {
        public string Data { get; private set; }
        public PosmInvestmentItemStatus Status { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public PosmInvestmentItemHistoryUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string data,
            PosmInvestmentItemStatus status
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            Data = data;
            Status = status;
        }
    }
}