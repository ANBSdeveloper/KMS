using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentMarketingConfirmProduceNewAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int PosmInvestmentId { get; private set; }
        public string DesignPhoto1 { get; private set; }
        public string DesignPhoto2 { get; private set; }  
        public string DesignPhoto3 { get; private set; }  
        public string DesignPhoto4 { get; private set; }
        public string Link { get; private set; }
        public string Note { get; private set; }

        public PosmInvestmentMarketingConfirmProduceNewAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmInvestmentId,
            string designPhoto1,
            string designPhoto2,
            string designPhoto3,
            string designPhoto4,
            string link,
            string note
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
			PosmInvestmentId = posmInvestmentId;
			DesignPhoto1 = designPhoto1;
			DesignPhoto2 = designPhoto2;
			DesignPhoto3 = designPhoto3;
			DesignPhoto4 = designPhoto4;
            Link = link;
            Note = note;
        }
    }
}