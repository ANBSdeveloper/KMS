using Cbms.Kms.Application.CustomerLocations.Commands;
using Cbms.Kms.Domain.CustomerLocations;
using Cbms.Mediator;

namespace Cbms.Kms.Application.CustomerLocations.CommandHandlers
{
    public class CustomerLocationDeleteCommandHandler : DeleteEntityCommandHandler<CustomerLocationDeleteCommand, CustomerLocation>
    {
        public CustomerLocationDeleteCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}