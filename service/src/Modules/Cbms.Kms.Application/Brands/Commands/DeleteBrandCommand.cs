using Cbms.Mediator;

namespace Cbms.Kms.Application.Brands.Commands
{
    public class DeleteBrandCommand : DeleteEntityCommand
    {
        public DeleteBrandCommand(int id) : base(id)
        {
        }
    }
}