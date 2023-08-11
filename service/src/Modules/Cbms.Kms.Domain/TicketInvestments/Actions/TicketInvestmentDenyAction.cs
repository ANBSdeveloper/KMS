﻿using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketInvestmentDenyAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public int UserId { get; private set; }

        public TicketInvestmentDenyAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int userId
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            UserId = userId;
        }
    }
}