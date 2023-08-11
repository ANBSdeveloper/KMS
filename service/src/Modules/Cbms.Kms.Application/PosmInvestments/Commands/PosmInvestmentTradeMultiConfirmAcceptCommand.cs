using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentTradeMultiConfirmAcceptCommand : CommandBase
    {
        public class PosmInvestmentTradeMultiConfirmAcceptDto
        {
            public string Note { get; set; }
            public List<int> Ids { get; set; }
        }
        public PosmInvestmentTradeMultiConfirmAcceptDto Data { get; set; }
    }
}
