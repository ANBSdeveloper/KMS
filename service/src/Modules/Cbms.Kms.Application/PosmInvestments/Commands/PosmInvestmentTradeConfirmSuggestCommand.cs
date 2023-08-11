using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentTradeConfirmSuggestCommand : CommandBase
    {
        public class PosmInvestmentTradeConfirmSuggesDto
        {
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentTradeConfirmSuggesDto Data { get; set; }
        public PosmInvestmentTradeConfirmSuggestCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
