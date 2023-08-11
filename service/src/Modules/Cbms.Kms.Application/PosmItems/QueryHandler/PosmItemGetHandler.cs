using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Kms.Application.PosmItems.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmItems.QueryHandler
{
    public class PosmItemGetHandler : QueryHandlerBase, IRequestHandler<PosmItemGet, PosmItemDto>
    {
        private readonly IRepository<PosmItem, int> _repository;
        private readonly AppDbContext _dbContext;

        public PosmItemGetHandler(IRequestSupplement supplement, AppDbContext dbContext, IRepository<PosmItem, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<PosmItemDto> Handle(PosmItemGet request, CancellationToken cancellationToken)
        {
            var entity = Mapper.Map<PosmItemDto>(await _repository.GetAsync(request.Id));
            entity.Catalogs = await _dbContext.PosmCatalogs.
                Where(p => p.PosmItemId == request.Id)
                .Select(p => new PosmCatalogDto()
                {
                    PosmItemId = p.PosmItemId,
                    Code = p.Code,
                    CreationTime= p.CreationTime,
                    CreatorUserId= p.CreatorUserId,
                    Id= p.Id,
                    LastModificationTime= p.LastModificationTime,
                    LastModifierUserId= p.LastModifierUserId,
                    Link = p.Link,
                    Name = p.Name
                }).ToListAsync();
            return entity;
        }
    }
}