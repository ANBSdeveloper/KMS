using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentRsmApproveCommand : CommandBase
    {
        public class PosmInvestmentRsmApproveDto
        {
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentRsmApproveDto Data { get; set; }

        public PosmInvestmentRsmApproveCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
