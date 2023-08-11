using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmPrices.Dto;
using Cbms.Kms.Application.PosmPrices.Query;
using Cbms.Kms.Domain.PosmPrices;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmPrices.QueryHandler
{
    public class PosmPriceHeaderGetHandler : QueryHandlerBase, IRequestHandler<PosmPriceHeaderGet, PosmPriceHeaderDto>
    {
        private readonly IRepository<PosmPriceHeader, int> _repository;
        private readonly AppDbContext _dbContext;

        public PosmPriceHeaderGetHandler(IRequestSupplement supplement, IRepository<PosmPriceHeader, int> repository, AppDbContext dbContext) : base(supplement)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<PosmPriceHeaderDto> Handle(PosmPriceHeaderGet request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAsync(request.Id);

            var details = await (from detail in _dbContext.PosmPriceDetails
                                        join item in _dbContext.PosmItems on detail.PosmItemId equals item.Id 
                                        where detail.PosmPriceHeaderId == entity.Id
                                        select new PosmPriceDetailDto()
                                        {
                                            Id= detail.Id,
                                            PosmItemId= detail.PosmItemId,
                                            CreationTime= detail.CreationTime,
                                            CreatorUserId= detail.CreatorUserId,
                                            LastModificationTime= detail.LastModificationTime,
                                            LastModifierUserId = item.LastModifierUserId,
                                            Price = detail.Price,
                                            Code = item.Code,
                                            Name = item.Name,
                                            UnitType = item.UnitType,
                                            CalcType = item.CalcType,
                                        }).ToListAsync();

            var entityDto = Mapper.Map<PosmPriceHeaderDto>(entity);
            entityDto.Details = details;
            return entityDto;
        }
    }
}