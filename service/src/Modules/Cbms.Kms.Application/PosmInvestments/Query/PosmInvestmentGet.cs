using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Query
{
    public class PosmInvestmentGet : EntityQuery<PosmInvestmentDto>
    {
        public PosmInvestmentGet(int id) : base(id)
        {
        }
    }
}