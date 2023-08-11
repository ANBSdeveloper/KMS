using System;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketGetByConsumerDto
    {
        public string TicketCode { get; set; }
        public DateTime? OperationDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
    }
}