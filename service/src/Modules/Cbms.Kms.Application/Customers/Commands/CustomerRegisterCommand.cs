using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Customers.Commands
{
    public class CustomerRegisterCommand : CommandBase
    {
        public CustomerRegisterDto Data { get; set; }
    }
}