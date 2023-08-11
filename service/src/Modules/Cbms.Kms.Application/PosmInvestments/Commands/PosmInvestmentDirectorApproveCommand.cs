using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentDirectorApproveCommand : CommandBase
    {
        public class PosmInvestmentDirectorApproveDto
        {
            public string Note { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentDirectorApproveDto Data { get; set; }

        public PosmInvestmentDirectorApproveCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
