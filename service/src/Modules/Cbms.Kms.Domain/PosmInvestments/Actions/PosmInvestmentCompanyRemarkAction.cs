using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{
    public class PosmInvestmentCompanyRemarkAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public decimal Remark { get; private set; }
        public int PosmInvestmentItemId { get; private set; }

        public PosmInvestmentCompanyRemarkAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmInvestmentItemId,
            decimal remark
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            PosmInvestmentItemId = posmInvestmentItemId;
            Remark = remark;
        }
    }
}