using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.Orders.Dto
{
    public class OrderListItemDto : AuditedEntityDto
    {
        public string OrderNumber { get; set; }
        public int? TicketInvestmentId { get; set; }
        public string TicketInvestmentCode { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get;set; }
        public string TicketCodes { get; set; }
        public int BranchId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ConsumerPhone { get; set; }
        public string ConsumerName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPoints { get; set; }
        public decimal TotalAvailablePoints { get; set; }
        public decimal TotalUsedPoints { get; set; }
    }
}