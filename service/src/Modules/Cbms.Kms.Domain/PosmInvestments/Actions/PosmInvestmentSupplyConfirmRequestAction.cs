using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentSupplyConfirmRequestAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public DateTime ConfirmDate { get; private set; } 
        public int VendorId { get; private set; }
        public string Note { get; private set; }
        public decimal ActualUnitPrice { get; private set; }
        public int PosmInvestmentItemId { get; private set; }

        public PosmInvestmentSupplyConfirmRequestAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmInvestmentItemId,
            DateTime confirmDate,
            int vendorId,
            string note,
            decimal actualUnitPrice
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            PosmInvestmentItemId = posmInvestmentItemId;
            ConfirmDate = confirmDate;
            VendorId = vendorId;
            Note = note;
            ActualUnitPrice = actualUnitPrice;
        }
    }
}