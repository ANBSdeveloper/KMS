using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentDirectorMultiApproveCommand : CommandBase
    {
        public class PosmInvestmentDirectorMultiApproveDto
        {
            public string Note { get; set; }
            public List<int> Ids { get; set; }
        }
        public PosmInvestmentDirectorMultiApproveDto Data { get; set; }
    }
}
