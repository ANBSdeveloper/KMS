using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductClasses.Commands
{
    public class DeleteProductClassCommand : DeleteEntityCommand
    {
        public DeleteProductClassCommand(int id) : base(id)
        {
        }
    }
}