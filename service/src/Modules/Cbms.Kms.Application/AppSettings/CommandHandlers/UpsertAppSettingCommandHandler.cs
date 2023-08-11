using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.AppSettings.Commands;
using Cbms.Kms.Application.AppSettings.Dto;
using Cbms.Kms.Application.AppSettings.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.AppSettings.Actions;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.AppSettings.CommandHandlers
{
    public class UpsertAppSettingCommandHandler : UpsertEntityCommandHandler<UpsertAppSettingCommand, GetAppSetting, AppSettingDto>
    {
        private readonly IRepository<AppSetting, int> _appSettingRepository;

        public UpsertAppSettingCommandHandler(IRequestSupplement supplement, IRepository<AppSetting, int> AppSettingRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _appSettingRepository = AppSettingRepository;
        }

        protected override async Task<AppSettingDto> HandleCommand(UpsertAppSettingCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            AppSetting entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _appSettingRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = AppSetting.Create();
                await _appSettingRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new AppSettingUpsertAction(
                entityDto.Code,
                entityDto.Value,
                entityDto.Description
            ));

            await _appSettingRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}
