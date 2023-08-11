using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentUpsertFinalSettlementCommand : CommandBase<TicketInvestmentDto>
    {
        public TicketInvestmentUpsertFinalSettlementDto Data { get; set; }
        public string HandleType { get; set; }

        public TicketInvestmentUpsertFinalSettlementCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
