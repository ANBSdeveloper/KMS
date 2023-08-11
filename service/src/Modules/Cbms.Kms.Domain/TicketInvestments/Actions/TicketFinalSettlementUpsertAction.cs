using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketFinalSettlementUpsertAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int UserId { get; private set; }
        public DateTime Date { get; private set; }
        public string Note { get; private set; }
        public int DecideUserId { get; private set; }
        public bool Complete { get; private set; }

        public TicketFinalSettlementUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int userId,
            string note,
            DateTime date,
            int decideUserId,
            bool complete
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            UserId = userId;
            Note = note;
            Date = date;
            DecideUserId = decideUserId;
            Complete = complete;
        }
    }
}