using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Mediator;
using System;

namespace Cbms.Kms.Application.PosmInvestments.Query
{
    public class PosmInvestmentItemGetListByCustomer : EntityPagingResultQuery<PosmInvestmentItemExtDto>
    {
        public int CustomerId { get; set; }
    }
}