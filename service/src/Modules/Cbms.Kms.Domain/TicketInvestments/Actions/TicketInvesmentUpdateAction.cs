using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketInvestmentUpdateAction : IEntityAction
    {
        public DateTime OperationDate { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public TicketInvestmentUpdateAction(IIocResolver iocResolver,
            ILocalizationSource localizationSource, DateTime operationDate)
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            OperationDate = operationDate;
        }
    }
}