using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Customers.Commands
{
    public class CustomerActivateKeyShopCommand : CommandBase
    {
        public CustomerActivateKeyShopDto Data { get; set; }
    }
}