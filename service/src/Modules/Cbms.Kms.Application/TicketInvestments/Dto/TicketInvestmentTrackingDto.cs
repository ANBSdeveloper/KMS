using Cbms.Dto;
using Cbms.Kms.Application.TicketInvestments.Dto;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments
{
    public class TicketInvestmentTrackingDto : EntityDto
    {
        public string Code { get; set; }
        public DateTime BuyBeginDate { get; set; }
        public DateTime BuyEndDate { get; set; }
        public DateTime IssueTicketBeginDate { get; set; }
        public DateTime IssueTicketEndDate { get; set; }
        public DateTime OperationDate { get; set; }
        public int SentTicketQuantity { get; set; }
        public int QrCodeQuantity { get; set; }
        public int ConsumerQuantity { get; set; }
        public List<TicketDto> Tickets { get; set; }
    }
}