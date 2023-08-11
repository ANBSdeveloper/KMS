using Cbms.Kms.Application.ProductPoints.Commands;
using Cbms.Kms.Domain.ProductPoints;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductPoints.CommandHandlers
{
    public class ProductPointDeleteCommandHandler : DeleteEntityCommandHandler<ProductPointDeleteCommand, ProductPoint>
    {
        public ProductPointDeleteCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}
