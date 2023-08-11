using Cbms.Kms.Application.Orders.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Orders.Query
{
    public class OrderDetailGetListByOrderId : EntityPagingResultQuery<OrderDetailDto>
    {
        public int OrderId { get; set; }
    }
}