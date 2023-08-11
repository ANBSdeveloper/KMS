using Cbms.Kms.Infrastructure.Migrations;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentTradeMultiDenyCommand : CommandBase
    {
        public class PosmInvestmentTradeMultiDenyDto
        {
            public string Note { get; set; }
            public List<int> Ids { get; set; }
        }
        public PosmInvestmentTradeMultiDenyDto Data { get; set; }
    }
}
