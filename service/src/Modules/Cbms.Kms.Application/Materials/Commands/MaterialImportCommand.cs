using Cbms.Mediator;
using Microsoft.AspNetCore.Http;

namespace Cbms.Kms.Application.Materials.Commands
{
    public class MaterialImportCommand : CommandBase
    {
        public MaterialImportCommand(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; private set; }
    }
}