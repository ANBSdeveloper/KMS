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
    public class StaffGetByUserHandler : QueryHandlerBase, IRequestHandler<StaffGetByUser, StaffDto>
    {
        private readonly IRepository<Staff, int> _repository;

        public StaffGetByUserHandler(IRequestSupplement supplement, IRepository<Staff, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<StaffDto> Handle(StaffGetByUser request, CancellationToken cancellationToken)
        {
            return Mapper.Map<StaffDto>(await _repository.FirstOrDefaultAsync(p=>p.UserId == Session.UserId));
        }
    }

}
