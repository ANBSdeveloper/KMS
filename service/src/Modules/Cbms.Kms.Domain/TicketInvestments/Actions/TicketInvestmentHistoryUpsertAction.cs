using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketInvestmentHistoryUpsertAction : IEntityAction
    {
        public string Data { get; private set; }
        public TicketInvestmentStatus Status { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public TicketInvestmentHistoryUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string data,
            TicketInvestmentStatus status
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            Data = data;
            Status = status;
        }
    }
}