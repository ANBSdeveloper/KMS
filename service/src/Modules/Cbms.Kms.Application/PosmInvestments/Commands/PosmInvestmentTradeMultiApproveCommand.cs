using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentTradeMultiApproveCommand : CommandBase
    {
        public class PosmInvestmentTradeMultiApproveDto
        {
            public string Note { get; set; }
            public List<int> Ids { get; set; }
        }
        public PosmInvestmentTradeMultiApproveDto Data { get; set; }
    }
}
