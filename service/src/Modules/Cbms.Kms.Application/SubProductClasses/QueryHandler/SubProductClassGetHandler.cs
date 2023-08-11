using Cbms.Domain.Repositories;
using Cbms.Kms.Application.SubProductClasses.Dto;
using Cbms.Kms.Application.SubProductClasses.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.SubProductClasses;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.SubProductClasses.QueryHandlers
{
    public class SubProductClassGetHandler : QueryHandlerBase, IRequestHandler<SubProductClassGet, SubProductClassDto>
    {
        private readonly IRepository<SubProductClass, int> _repository;

        public SubProductClassGetHandler(IRequestSupplement supplement, IRepository<SubProductClass, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<SubProductClassDto> Handle(SubProductClassGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<SubProductClassDto>(await _repository.GetAsync(request.Id));
        }
    }
}