using Cbms.Kms.Application.CustomerLocations.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.CustomerLocations.Commands
{
    public class CustomerLocationUpsertCommand : UpsertEntityCommand<CustomerLocationUpsertDto, CustomerLocationDto>
    {
        public CustomerLocationUpsertCommand(CustomerLocationUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}