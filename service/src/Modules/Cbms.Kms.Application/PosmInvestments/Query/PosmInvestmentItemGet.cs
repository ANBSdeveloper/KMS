using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Query
{
    public class PosmInvestmentItemGet : EntityQuery<PosmInvestmentItemDto>
    {
        public PosmInvestmentItemGet(int id) : base(id)
        {
        }
    }
}