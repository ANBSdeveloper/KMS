using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductUnits.Commands
{
    public class DeleteProductUnitCommand : DeleteEntityCommand
    {
        public DeleteProductUnitCommand(int id) : base(id)
        {
        }
    }
}