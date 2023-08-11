using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Orders;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Orders.Dto
{
    [AutoMap(typeof(Order))]
    public class OrderDto : AuditedEntityDto
    {
        public string OrderNumber { get; set; }
        public int? TicketInvestmentId { get; set; }
        public int CustomerId { get; set; }
        public int BranchId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ConsumerPhone { get; set; }
        public string ConsumerName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPoints { get; set; }
        public decimal TotalAvailablePoints { get; set; }
        public decimal TotalUsedPoints { get; set; }
        [IgnoreMap]
        public List<OrderDetailDto> OrderDetails { get; set; }
        [IgnoreMap]
        public List<OrderTicketDto> Tickets { get; set; }
    }
}