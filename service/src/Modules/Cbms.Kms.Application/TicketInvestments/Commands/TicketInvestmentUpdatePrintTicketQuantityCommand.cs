using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Commands
{
    public class TicketInvestmentUpdatePrintTicketQuantityCommand : CommandBase
    {
        public int Id { get; set; }
        public List<int> Data { get; set; }
    }
}