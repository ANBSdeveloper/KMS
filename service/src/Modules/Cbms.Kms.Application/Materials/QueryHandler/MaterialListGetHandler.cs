using Cbms.Kms.Application.Materials.Dto;
using Cbms.Kms.Application.Materials.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Materials.QueryHandlers
{
    public class MaterialGetListHandler : QueryHandlerBase, IRequestHandler<MaterialGetList, PagingResult<MaterialListItemDto>>
    {
        private readonly AppDbContext _dbContext;

        public MaterialGetListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<MaterialListItemDto>> Handle(MaterialGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from material in _dbContext.Materials
                        join materialType in _dbContext.MaterialTypes on material.MaterialTypeId equals materialType.Id
                        select new MaterialListItemDto()
                        {
                            CreationTime = material.CreationTime,
                            CreatorUserId = material.CreatorUserId,
                            Name = material.Name,
                            Id = material.Id,
                            IsActive = material.IsActive,
                            LastModificationTime = material.LastModificationTime,
                            LastModifierUserId = material.LastModifierUserId,
                            MaterialTypeId = materialType.Id,
                            MaterialTypeName = materialType.Name,
                            Code = material.Code,
                            Description = material.Description,
                            IsDesign = material.IsDesign,
                            Value = material.Value
                        };

            query = query
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive)
                .WhereIf(request.MaterialTypeId.HasValue, x => x.MaterialTypeId == request.MaterialTypeId)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                    x.Name.Contains(keyword) || x.MaterialTypeName.Contains(keyword));

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
            return new PagingResult<MaterialListItemDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}