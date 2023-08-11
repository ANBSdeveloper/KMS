﻿using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmnetGetRequestListByUser : EntityPagingResultQuery<TicketInvestmentListItemDto>
    {
        public int? StaffId { get; set; }
    }
}