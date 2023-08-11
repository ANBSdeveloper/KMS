using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentTradeDenyCommand : CommandBase
    {
        public class PosmInvestmentTradeDenyDto
        {
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentTradeDenyDto Data { get; set; }

        public PosmInvestmentTradeDenyCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
