using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.CustomerLocations.Commands;
using Cbms.Kms.Application.CustomerLocations.Dto;
using Cbms.Kms.Application.CustomerLocations.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.CustomerLocations;
using Cbms.Kms.Domain.CustomerLocations.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.CustomerLocations.CommandHandlers
{
    public class CustomerLocationUpsertCommandHandler : UpsertEntityCommandHandler<CustomerLocationUpsertCommand, CustomerLocationGet, CustomerLocationDto>
    {
        private readonly IRepository<CustomerLocation, int> _customerLocationRepository;

        public CustomerLocationUpsertCommandHandler(IRequestSupplement supplement, IRepository<CustomerLocation, int> customerLocationRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _customerLocationRepository = customerLocationRepository;
        }

        protected override async Task<CustomerLocationDto> HandleCommand(CustomerLocationUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            CustomerLocation entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _customerLocationRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = CustomerLocation.Create();
                await _customerLocationRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new CustomerLocationUpsertAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.IsActive
            ));

            await _customerLocationRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}