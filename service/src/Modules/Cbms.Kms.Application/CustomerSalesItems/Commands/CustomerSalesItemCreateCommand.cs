using Cbms.Kms.Application.CustomerSalesItems.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.CustomerSalesItems.Commands
{
    public class CustomerSalesItemCreateCommand : CommandBase<string>
    {
        public CustomerSalesItemCreateDto Data { get; set; }
    }
}