using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Geography.Wards.Dto;
using Cbms.Kms.Application.Geography.Wards.Query;
using Cbms.Kms.Domain.Geography.Wards;
using Cbms.Mediator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Geography.Wards.QueryHandler
{
    public class GetWardHandler : QueryHandlerBase, IRequestHandler<GetWard, WardDto>
    {
        private readonly IRepository<Ward, int> _repository;

        public GetWardHandler(IRequestSupplement supplement, IRepository<Ward, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<WardDto> Handle(GetWard request, CancellationToken cancellationToken)
        {
            return Mapper.Map<WardDto>(await _repository.GetAsync(request.Id));
        }
    }
}
