using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Mediator;
using MediatR;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Query
{
    public class TicketGetByConsumer : QueryBase, IRequest<List<TicketGetByConsumerDto>>
    {
        public string Phone { get; set; }
    }
}