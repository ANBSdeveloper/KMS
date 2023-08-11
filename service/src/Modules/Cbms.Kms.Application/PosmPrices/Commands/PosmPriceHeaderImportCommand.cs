using Cbms.Mediator;
using Microsoft.AspNetCore.Http;

namespace Cbms.Kms.Application.PosmPrices.Commands
{
    public class PosmPriceHeaderImportCommand : CommandBase
    {
        public PosmPriceHeaderImportCommand(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; private set; }
    }
}