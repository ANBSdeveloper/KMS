using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketGetListByTicketInvestmentId : EntityPagingResultQuery<TicketListDto>
    {
        public int TicketInvestmentId { get; set; }
    }
}