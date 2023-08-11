using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentTradeApproveRequestAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public string Note { get; private set; }
        public PosmInvestmentTradeApproveRequestAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string note
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            Note = note;
        }
    }
}