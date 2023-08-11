using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmnetGetListByTime : EntityPagingResultQuery<TicketInvestmentListItemDto>
    {
        public List<int> Status { get; set; }
        public int? RsmStaffId { get;  set; }
        public int? AsmStaffId { get;  set; }
        public int? SalesSupervisorStaffId { get;  set; }
        public DateTime FromDate { get;  set; }
        public DateTime ToDate { get;  set; }
        public TicketInvestmnetGetListByTime()
        {
            Status = new List<int>();
        }
        public bool ByOperationDate { get; set; }
    }
}