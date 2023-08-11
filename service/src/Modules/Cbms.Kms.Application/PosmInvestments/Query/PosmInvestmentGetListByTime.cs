using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Mediator;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Query
{
    public class PosmInvestmentGetListByTime : EntityPagingResultQuery<PosmInvestmentListDto>
    {
        public List<int> Status { get; set; }
        public int? RsmStaffId { get;  set; }
        public int? AsmStaffId { get;  set; }
        public int? SalesSupervisorStaffId { get;  set; }
        public DateTime FromDate { get;  set; }
        public DateTime ToDate { get;  set; }
        public PosmInvestmentGetListByTime()
        {
            Status = new List<int>();
        }
    }
}