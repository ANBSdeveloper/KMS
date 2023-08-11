using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentAsmConfirmSuggestCommand : CommandBase
    {
        public class PosmInvestmentAsmConfirmSuggesDto
        {
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentAsmConfirmSuggesDto Data { get; set; }
        public PosmInvestmentAsmConfirmSuggestCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
