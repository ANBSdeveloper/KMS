using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Brands.Commands;
using Cbms.Kms.Application.Brands.Dto;
using Cbms.Kms.Application.Brands.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Brands;
using Cbms.Kms.Domain.Brands.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Brands.CommandHandlers
{
    public class UpsertBrandCommandHandler : UpsertEntityCommandHandler<UpsertBrandCommand, GetBrand, BrandDto>
    {
        private readonly IRepository<Brand, int> _brandRepository;

        public UpsertBrandCommandHandler(IRequestSupplement supplement, IRepository<Brand, int> brandRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _brandRepository = brandRepository;
        }

        protected override async Task<BrandDto> HandleCommand(UpsertBrandCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            Brand entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _brandRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = Brand.Create();
                await _brandRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new BrandUpsertAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.IsActive
            ));

            await _brandRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}