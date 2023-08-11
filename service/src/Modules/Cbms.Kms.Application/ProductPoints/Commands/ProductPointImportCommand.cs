using Cbms.Mediator;
using Microsoft.AspNetCore.Http;

namespace Cbms.Kms.Application.Materials.Commands
{
    public class ProductPointImportCommand : CommandBase
    {
        public ProductPointImportCommand(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; private set; }
    }
}