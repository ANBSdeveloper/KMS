using Cbms.Kms.Application.PosmItems.Query;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmItems.QueryHandler
{
    public class PosmItemPriceGetHandler : QueryHandlerBase, IRequestHandler<PosmItemPriceGet, decimal>
    {
        private readonly IPosmInvestmentManager _investmentManager;

        public PosmItemPriceGetHandler(IRequestSupplement supplement, IPosmInvestmentManager investmentManager) : base(supplement)
        {
            _investmentManager = investmentManager;
        }

        public async Task<decimal> Handle(PosmItemPriceGet request, CancellationToken cancellationToken)
        {

            return await _investmentManager.GetPriceAsync(request.PosmItemId);
        }
    }
}