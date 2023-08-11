using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductPoints.Commands
{
    public class ProductPointDeleteCommand : DeleteEntityCommand
    {
        public ProductPointDeleteCommand(int id) : base(id)
        {
        }
    }
}
