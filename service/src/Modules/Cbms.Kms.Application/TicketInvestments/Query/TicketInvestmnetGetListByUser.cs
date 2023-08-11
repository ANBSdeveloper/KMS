using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmnetGetListByUser : EntityPagingResultQuery<TicketInvestmentListItemDto>
    {
        public List<int> Status { get; set; }
        public int? CycleId { get; set; }
        public int? StaffId { get; set; }

        public TicketInvestmnetGetListByUser()
        {
            Status = new List<int>();
        }
    }
}