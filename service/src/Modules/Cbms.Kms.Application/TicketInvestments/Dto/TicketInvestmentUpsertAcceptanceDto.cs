using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentUpsertAcceptanceDto : EntityDto
    {
        public DateTime AcceptanceDate { get; set; }
        public string Note { get; set; }
        public decimal ActualSalesAmount { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }
    }
}