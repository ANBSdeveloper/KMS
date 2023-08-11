using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;
using System;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketInvestmnetGetListByCustomer : EntityPagingResultQuery<TicketInvestmentListItemDto>
    {
        public int CustomerId { get; set; }
    }
}