using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentMarketingConfirmProduceNewCommand : CommandBase
    {
        public class PosmInvestmentMarketingConfirmProduceNewDto
        {
			public int PosmInvestmentId { get; set; }
			public string DesignPhoto1 { get; set; }
            public string DesignPhoto2 { get; set; }
            public string DesignPhoto3 { get; set; }
            public string DesignPhoto4 { get; set; }
            public string Link { get; set; }
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentMarketingConfirmProduceNewDto Data { get; set; }

        public PosmInvestmentMarketingConfirmProduceNewCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
