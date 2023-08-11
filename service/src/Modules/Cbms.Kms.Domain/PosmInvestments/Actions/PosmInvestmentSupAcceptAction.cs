using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentSupAcceptAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int PosmInvestmentItemId { get; private set; }
        public string Note { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }  
        public string Photo3 { get; private set; }  
        public string Photo4 { get; private set; }
        public PosmInvestmentSupAcceptAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmInvestmentItemId,
            string note,
            string photo1,
            string photo2,
            string photo3,
            string photo4
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            PosmInvestmentItemId= posmInvestmentItemId;
            Note = note;
            Photo1 = photo1;
            Photo2 = photo2;
            Photo3 = photo3;
            Photo4 = photo4;
        }
    }
}