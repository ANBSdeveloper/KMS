using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Vendors.Commands;
using Cbms.Kms.Application.Vendors.Dto;
using Cbms.Kms.Application.Vendors.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Vendors;
using Cbms.Kms.Domain.Vendors.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Vendors.CommandHandlers
{
    public class VendorUpsertCommandHandler : UpsertEntityCommandHandler<VendorUpsertCommand, VendorGet, VendorDto>
    {
        private readonly IRepository<Vendor, int> _vendorRepository;

        public VendorUpsertCommandHandler(IRequestSupplement supplement, IRepository<Vendor, int> vendorRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _vendorRepository = vendorRepository;
        }

        protected override async Task<VendorDto> HandleCommand(VendorUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            Vendor entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _vendorRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = Vendor.Create();
                await _vendorRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new VendorUpsertAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.Address,
                entityDto.Phone,
                entityDto.IsActive,
                entityDto.ZoneId,
                entityDto.TaxReg,
                entityDto.Representative
            ));

            await _vendorRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}