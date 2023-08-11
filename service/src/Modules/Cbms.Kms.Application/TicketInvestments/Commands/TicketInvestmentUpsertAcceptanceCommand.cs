using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentUpsertAcceptanceCommand : CommandBase<TicketInvestmentDto>
    {
        public TicketInvestmentUpsertAcceptanceDto Data { get; set; }
        public string HandleType { get; set; }

        public TicketInvestmentUpsertAcceptanceCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
