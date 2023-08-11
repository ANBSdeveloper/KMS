using Cbms.Kms.Application.Orders.Dto;
using Cbms.Mediator;
using System;

namespace Cbms.Kms.Application.Orders.Query
{
    public class OrderGetList : EntityPagingResultQuery<OrderListItemDto>
    {
        public int? CustomerId { get;  set; }
        public int? RsmStaffId { get;  set; }
        public int? AsmStaffId { get;  set; }
        public int? SalesSupervisorStaffId { get;  set; }
        public DateTime FromDate { get;  set; }
        public DateTime ToDate { get;  set; }
    }
}