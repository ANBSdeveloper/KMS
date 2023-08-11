using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketCreateAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public string ConsumerPhone { get; private set; }
        public string ConsumerName { get; private set; }
        // use for check duplicate 
        public int TickteInvestmentId { get; private set; }
        public TicketCreateAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string phone,
            string fullName,
            int ticketInvestmentId
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            ConsumerPhone = phone;
            ConsumerName = fullName;
            TickteInvestmentId = ticketInvestmentId;
        }
    }
}