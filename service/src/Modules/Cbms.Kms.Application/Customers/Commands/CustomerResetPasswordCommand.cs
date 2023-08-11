using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Customers.Commands
{
    public class CustomerResetPasswordCommand : CommandBase
    {
        public CustomerResetPasswordDto Data { get; set; }
    }
}