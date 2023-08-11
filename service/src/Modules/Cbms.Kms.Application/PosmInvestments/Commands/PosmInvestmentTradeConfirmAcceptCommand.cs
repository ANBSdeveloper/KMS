using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentTradeConfirmAcceptCommand : CommandBase
    {
        public class PosmInvestmentTradeConfirmAcceptDto
        {
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentTradeConfirmAcceptDto Data { get; set; }
        public PosmInvestmentTradeConfirmAcceptCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
