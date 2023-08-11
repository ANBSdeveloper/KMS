using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentTradeApproveCommand : CommandBase
    {
        public class PosmInvestmentTradeApproveDto
        {
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentTradeApproveDto Data { get; set; }

        public PosmInvestmentTradeApproveCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
