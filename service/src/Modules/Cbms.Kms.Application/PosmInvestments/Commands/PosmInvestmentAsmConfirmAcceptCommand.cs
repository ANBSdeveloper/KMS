using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentAsmConfirmAcceptCommand : CommandBase
    {
        public class PosmInvestmentAsmConfirmAcceptDto
        {
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentAsmConfirmAcceptDto Data { get; set; }
        public PosmInvestmentAsmConfirmAcceptCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
