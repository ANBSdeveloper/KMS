using Cbms.Kms.Application.Orders.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Orders.Query
{
    public class OrderGet : EntityQuery<OrderDto>
    {
        public OrderGet(int id) : base(id)
        {
        }
    }
}