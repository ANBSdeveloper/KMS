using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Materials.Dto;
using Cbms.Kms.Application.Materials.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Materials;
using Cbms.Mediator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Materials.QueryHandler
{
    public class MaterialGetHandler : QueryHandlerBase, IRequestHandler<MaterialGet, MaterialDto>
    {
        private readonly IRepository<Material, int> _repository;

        public MaterialGetHandler(IRequestSupplement supplement, IRepository<Material, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<MaterialDto> Handle(MaterialGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<MaterialDto>(await _repository.GetAsync(request.Id));
        }
    }
}
