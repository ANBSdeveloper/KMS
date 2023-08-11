using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentSupSuggestCommand : CommandBase
    {
        public class PosmInvestmentSupSuggesDto
        {
            public int PosmInvestmentItemId { get; set; }
            public string Reason { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentSupSuggesDto Data { get; set; }
        public PosmInvestmentSupSuggestCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
