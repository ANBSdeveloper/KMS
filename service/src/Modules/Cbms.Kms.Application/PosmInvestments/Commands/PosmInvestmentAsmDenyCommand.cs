using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentAsmDenyCommand : CommandBase
    {
        public class PosmInvestmentAsmDenyDto
        {
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentAsmDenyDto Data { get; set; }

        public PosmInvestmentAsmDenyCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
