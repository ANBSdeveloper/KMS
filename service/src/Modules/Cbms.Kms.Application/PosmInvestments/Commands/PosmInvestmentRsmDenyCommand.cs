using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentRsmDenyCommand : CommandBase
    {
        public class PosmInvestmentRsmDenyDto
        {
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentRsmDenyDto Data { get; set; }

        public PosmInvestmentRsmDenyCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
