using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketConsumerRewardDetailUpsertAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int TicketId { get; private set; }
        public string Note { get; private set; }

        public TicketConsumerRewardDetailUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int ticketId,
            string note
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            TicketId = ticketId;
            Note = note;
        }
    }
}