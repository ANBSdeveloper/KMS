using Cbms.Domain.Entities;
using Cbms.Kms.Application.PosmInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.QueryHandlers
{
    public class PosmInvestmentItemOperationImageGetHandler : QueryHandlerBase, IRequestHandler<PosmInvestmentItemOperationImageGet, string>
    {
        private readonly AppDbContext _dbContext;

        public PosmInvestmentItemOperationImageGetHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _dbContext = dbContext;
        }

        public async Task<string> Handle(PosmInvestmentItemOperationImageGet request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.PosmInvestmentItems
                .Where(p => p.Id == request.PosmInvestmentItemid).FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), request.PosmInvestmentItemid);
            }

            if (request.Index == 0)
            {
                return entity.OperationPhoto1;
            }
            if (request.Index == 1)
            {
                return entity.OperationPhoto2;
            }
            if (request.Index == 2)
            {
                return entity.OperationPhoto3;
            }
            if (request.Index == 3)
            {
                return entity.OperationPhoto4;
            }
            return "";
        }
    }
}