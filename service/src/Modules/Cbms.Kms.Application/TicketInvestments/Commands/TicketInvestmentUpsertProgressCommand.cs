using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentUpsertProgressCommand : UpsertEntityCommand<TicketInvestmentUpsertProgressDto , TicketInvestmentDto>
    {
        public TicketInvestmentUpsertProgressCommand(TicketInvestmentUpsertProgressDto  data, string handleType) : base(data, handleType)
        {
        }

        public TicketInvestmentUpsertProgressCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }

        public TicketInvestmentUpsertProgressCommand WithProgressId(int ticketUpdateId)
        {
            Data.ProgressId = ticketUpdateId;
            return this;
        }
    }
}
