using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentOperateCommand : CommandBase<TicketInvestmentDto>
    {
        public TicketInvestmentOperateDto Data { get; set; }
        public string HandleType { get; set; }

        public TicketInvestmentOperateCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
