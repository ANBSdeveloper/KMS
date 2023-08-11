using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Customers.Commands
{
    public class CustomerCheckOtpCommand : CommandBase
    {
        public CustomerCheckOtpDto Data { get; set; }
    }
}