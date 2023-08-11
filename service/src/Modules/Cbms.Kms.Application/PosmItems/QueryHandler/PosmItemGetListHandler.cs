using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Kms.Application.PosmItems.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Cbms.Kms.Application.PosmItems.QueryHandler
{
    public class PosmItemGetListHandler : QueryHandlerBase, IRequestHandler<PosmItemGetList, PagingResult<PosmItemListDto>>
    {
        private readonly AppDbContext _dbContext;
        public PosmItemGetListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<PosmItemListDto>> Handle(PosmItemGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from item in _dbContext.PosmItems
                        join clazz in _dbContext.PosmClasses on item.PosmClassId equals clazz.Id
                        select new PosmItemListDto()
                        {
                            PosmClassId = clazz.Id,
                            CalcType = item.CalcType,
                            Code = item.Code,
                            CreationTime = item.CreationTime,
                            CreatorUserId = item.CreatorUserId,
                            Id = item.Id,
                            IsActive = item.IsActive,
                            LastModificationTime = item.LastModificationTime,
                            LastModifierUserId = item.LastModifierUserId,
                            Name = item.Name,
                            PosmClassName = clazz.Name,
                            UnitType = item.UnitType,
                        };

            query = query
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive)
                .WhereIf(request.PosmClassId.HasValue, x => x.PosmClassId == request.PosmClassId)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                   EF.Functions.Like(x.Name, $"{keyword}%") || EF.Functions.Like(x.PosmClassName, $"{keyword}%"));
            //.WhereIf(!string.IsNullOrEmpty(request.ProductClassCode), x => x.ProductClassCode == request.ProductClassCode);

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
            return new PagingResult<PosmItemListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}