using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentAsmApproveCommand : CommandBase
    {
        public class PosmInvestmentAsmApproveDto
        {
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentAsmApproveDto Data { get; set; }

        public PosmInvestmentAsmApproveCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
