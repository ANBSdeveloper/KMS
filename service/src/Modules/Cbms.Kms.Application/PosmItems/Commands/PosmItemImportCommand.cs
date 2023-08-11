using Cbms.Mediator;
using Microsoft.AspNetCore.Http;

namespace Cbms.Kms.Application.PosmItems.Commands
{
    public class PosmItemImportCommand : CommandBase
    {
        public PosmItemImportCommand(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; private set; }
    }
}