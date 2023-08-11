using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentRsmApproveRequestAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public string Note { get; private set; }    

        public PosmInvestmentRsmApproveRequestAction(
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