using Cbms.Kms.Application.Orders.Dto;
using MediatR;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Orders.Query
{
    public class OrderGetListForShop: IRequest<List<OrderListItemDto>>
    {
        public int TicketInvestmentId { get;  set; }
    }
}