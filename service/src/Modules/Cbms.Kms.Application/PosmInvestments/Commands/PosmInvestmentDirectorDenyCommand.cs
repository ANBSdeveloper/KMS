using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentDirectorDenyCommand : CommandBase
    {
        public class PosmInvestmentDirectorDenyDto
        {
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentDirectorDenyDto Data { get; set; }

        public PosmInvestmentDirectorDenyCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
