using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentRsmConfirmSuggestCommand : CommandBase
    {
        public class PosmInvestmentRsmConfirmSuggesDto
        {
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentRsmConfirmSuggesDto Data { get; set; }
        public PosmInvestmentRsmConfirmSuggestCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
