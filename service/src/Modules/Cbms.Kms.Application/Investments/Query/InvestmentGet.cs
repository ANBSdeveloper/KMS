using Cbms.Kms.Application.Investments.Dto;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Investments.Query
{
    public class InvestmentGet : QueryBase, IRequest<InvestmentDto>
    {
       public int CustomerId { get; set; }
    }
}