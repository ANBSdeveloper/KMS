using Cbms.Kms.Application.PosmPrices.Dto;
using Cbms.Kms.Application.PosmPrices.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmPrices.QueryHandler
{
    public class PosmPriceHeaderGetListHandler : QueryHandlerBase, IRequestHandler<PosmPriceHeaderGetList, PagingResult<PosmPriceHeaderListDto>>
    {
        private readonly AppDbContext _dbContext;
        public PosmPriceHeaderGetListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }
        public async Task<PagingResult<PosmPriceHeaderListDto>> Handle(PosmPriceHeaderGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from posmPrices in _dbContext.PosmPriceeHeaders
                        select new PosmPriceHeaderListDto()
                        {
                            Code = posmPrices.Code,
                            Name = posmPrices.Name,
                            Id = posmPrices.Id,
                            FromDate = posmPrices.FromDate,
                            ToDate = posmPrices.ToDate,
                            IsActive = posmPrices.IsActive,
                        };                       


            query = query 
                    .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword))
                    .WhereIf(request.IsActive.HasValue, x => request.IsActive == x.IsActive)
                    .WhereIf(request.FromDate.HasValue && request.ToDate.HasValue, x => x.FromDate >= request.FromDate.Value && x.FromDate <= request.ToDate.Value);

            int totalCount = query.Count();
            query = query.SortFromString(request.Sort);
            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<PosmPriceHeaderListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}