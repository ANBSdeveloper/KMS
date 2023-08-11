using Cbms.Mediator;
using Microsoft.AspNetCore.Http;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{
    public class PosmInvestmentImportHistoryCommand : CommandBase
    {
        public PosmInvestmentImportHistoryCommand(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; private set; }
    }
}