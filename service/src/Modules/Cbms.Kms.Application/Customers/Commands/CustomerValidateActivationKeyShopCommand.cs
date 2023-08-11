using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Customers.Commands
{
    public class CustomerValidateActivationKeyShopCommand : CommandBase
    {
        public CustomerValidateActivationKeyShopDto Data { get; set; }
    }
}