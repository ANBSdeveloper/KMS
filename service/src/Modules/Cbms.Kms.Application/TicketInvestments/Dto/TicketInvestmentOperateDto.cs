using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentOperateDto: EntityDto
    {
        public DateTime OperationDate { get; set; }
        public string Note { get; set; }
        public int StockQuantity { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }
    }
}