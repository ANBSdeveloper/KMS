using Cbms.Kms.Application.Orders.Dto;
using Cbms.Mediator;
namespace Cbms.Kms.Application.Orders.Commands
{
    public class OrderCreateCommand : CommandBase<OrderDto>
    {
        public OrderCreateDto Data { get; set; }
    }
}
