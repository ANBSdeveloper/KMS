using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Vendors.Dto;
using Cbms.Kms.Application.Vendors.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Vendors;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Vendors.QueryHandler
{
    public class VendorGetHandler : QueryHandlerBase, IRequestHandler<VendorGet, VendorDto>
    {
        private readonly IRepository<Vendor, int> _repository;

        public VendorGetHandler(IRequestSupplement supplement, IRepository<Vendor, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<VendorDto> Handle(VendorGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<VendorDto>(await _repository.GetAsync(request.Id));
        }
    }
}