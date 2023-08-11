using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketGenerateAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public Action<Ticket> GetTicket { get; private set; }
        public string ConsumerPhone { get; private set; }
        public string ConsumerName { get; private set; }

        public TicketGenerateAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string consumerPhone,
            string consumerName,
            Action<Ticket> getTicket
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            GetTicket = getTicket;
            ConsumerPhone = consumerPhone;
            ConsumerName = consumerName;
        }
    }
}