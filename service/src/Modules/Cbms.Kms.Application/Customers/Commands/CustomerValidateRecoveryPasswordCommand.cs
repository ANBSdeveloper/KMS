using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Customers.Commands
{
    public class CustomerValidateRecoveryPasswordCommand : CommandBase
    {
        public CustomerValidateRecoveryPasswordDto Data { get; set; }
    }
}