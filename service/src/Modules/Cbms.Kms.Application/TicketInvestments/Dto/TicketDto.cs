using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.TicketInvestments;
using System;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    [AutoMap(typeof(Ticket))]
    public class TicketDto : EntityDto
    {
        public string Code { get; set; }
        public string ConsumerPhone { get; set; }
        public string ConsumerName { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? PrintDate { get; set; }
        public int PrintCount { get; set; }
        public int? LastPrintUserId { get; set; }  
        public int TicketInvestmentId { get; set; }
        public string LastPrintUserName { get; set; }
    }
}