using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentHistoryDto: EntityDto
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public string Data { get; set; }
        public DateTime CreationTime { get; set; }
    }
}