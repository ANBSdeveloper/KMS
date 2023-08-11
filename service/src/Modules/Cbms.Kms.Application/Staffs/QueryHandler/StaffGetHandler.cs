using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Kms.Application.Staffs.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Staffs;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Staffs.QueryHandler
{
    public class StaffGetHandler : QueryHandlerBase, IRequestHandler<StaffGet, StaffDto>
    {
        private readonly IRepository<Staff, int> _repository;

        public StaffGetHandler(IRequestSupplement supplement, IRepository<Staff, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<StaffDto> Handle(StaffGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<StaffDto>(await _repository.GetAsync(request.Id));
        }
    }

}
