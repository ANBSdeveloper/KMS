using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentMarketingConfirmProduceCommand : CommandBase
    {
        public class PosmInvestmentMarketingConfirmProduceDto
        {
            public int PosmInvestmentItemId { get; set; }
            public string Photo1 { get; set; }
            public string Photo2 { get; set; }
            public string Photo3 { get; set; }
            public string Photo4 { get; set; }
            public string Link { get; set; }
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentMarketingConfirmProduceDto Data { get; set; }

        public PosmInvestmentMarketingConfirmProduceCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
