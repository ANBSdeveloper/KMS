using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentUpdateDto: EntityDto
    {
        public DateTime OperationDate { get; set; }
    }
}