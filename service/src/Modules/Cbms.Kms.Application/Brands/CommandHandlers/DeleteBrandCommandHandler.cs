using Cbms.Kms.Application.Brands.Commands;
using Cbms.Kms.Domain.Brands;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Brands.CommandHandlers
{
    public class DeleteBrandCommandHandler : DeleteEntityCommandHandler<DeleteBrandCommand, Brand>
    {
        public DeleteBrandCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}