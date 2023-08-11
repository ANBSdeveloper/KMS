using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Branches.Dto;
using Cbms.Kms.Application.Branches.Query;
using Cbms.Kms.Domain.Branches;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Branches.QueryHandler
{
    public class GetBranchHandler : QueryHandlerBase, IRequestHandler<GetBranch, BranchDto>
    {
        private readonly IRepository<Branch, int> _repository;

        public GetBranchHandler(IRequestSupplement supplement, IRepository<Branch, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<BranchDto> Handle(GetBranch request, CancellationToken cancellationToken)
        {
            return Mapper.Map<BranchDto>(await _repository.GetAsync(request.Id));
        }
    }
}