using Cbms.Kms.Application.ProductUnits.Commands;
using Cbms.Kms.Domain.ProductUnits;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductUnits.CommandHandlers
{
    public class DeleteProductUnitCommandHandler : DeleteEntityCommandHandler<DeleteProductUnitCommand, ProductUnit>
    {
        public DeleteProductUnitCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
            LocalizationSourceName = "Stock";
        }
    }
}