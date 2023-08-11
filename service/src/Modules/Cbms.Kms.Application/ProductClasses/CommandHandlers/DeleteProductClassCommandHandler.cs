using Cbms.Kms.Application.ProductClasses.Commands;
using Cbms.Kms.Domain.ProductClasses;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductClasses.CommandHandlers
{
    public class DeleteProductClassCommandHandler : DeleteEntityCommandHandler<DeleteProductClassCommand, ProductClass>
    {
        public DeleteProductClassCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
            LocalizationSourceName = "Stock";
        }
    }
}