using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketAcceptanceCustomerDevelopmentRemarkAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public decimal Remark { get; private set; }

        public TicketAcceptanceCustomerDevelopmentRemarkAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            decimal remark
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            Remark = remark;
        }
    }
}