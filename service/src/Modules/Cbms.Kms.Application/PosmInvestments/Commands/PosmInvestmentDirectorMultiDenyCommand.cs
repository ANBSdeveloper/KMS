using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentDirectorMultiDenyCommand : CommandBase
    {
        public class PosmInvestmentDirectorMultiDenyDto
        {
            public string Note { get; set; }
            public List<int> Ids { get; set; }
        }
        public PosmInvestmentDirectorMultiDenyDto Data { get; set; }
    }
}
