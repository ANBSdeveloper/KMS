using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketInvesmentUpsertPrintTicketQuantityAction : IEntityAction
    {
        public int? StaffId { get; private set; }
        public List<int> TicketId { get; private set; }

        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public TicketInvesmentUpsertPrintTicketQuantityAction(IIocResolver iocResolver,
            ILocalizationSource localizationSource, int? staffId, List<int> ticketId)
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            StaffId = staffId;
            TicketId = ticketId;
        }
    }
}