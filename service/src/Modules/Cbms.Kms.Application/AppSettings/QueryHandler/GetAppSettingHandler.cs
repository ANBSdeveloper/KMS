using Cbms.Domain.Repositories;
using Cbms.Kms.Application.AppSettings.Dto;
using Cbms.Kms.Application.AppSettings.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Mediator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.AppSettings.QueryHandler
{
    public class GetAppSettingHandler : QueryHandlerBase, IRequestHandler<GetAppSetting, AppSettingDto>
    {
        private readonly IRepository<AppSetting, int> _repository;

        public GetAppSettingHandler(IRequestSupplement supplement, IRepository<AppSetting, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<AppSettingDto> Handle(GetAppSetting request, CancellationToken cancellationToken)
        {
            return Mapper.Map<AppSettingDto>(await _repository.GetAsync(request.Id));
        }
    }
}
