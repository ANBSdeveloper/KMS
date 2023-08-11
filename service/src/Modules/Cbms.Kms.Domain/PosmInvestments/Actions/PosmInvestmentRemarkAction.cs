using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentRemarkAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int SalesRemark { get; private set; }
        public int CompanyRemark { get; private set; }
        public PosmInvestmentRemarkAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmInvestmentItemId,
            int salesRemark,
            int companyRemark
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            SalesRemark = salesRemark;
            CompanyRemark = companyRemark;
        }
    }
}