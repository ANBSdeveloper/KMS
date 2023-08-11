using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.InvestmentSettings.Commands;
using Cbms.Kms.Application.InvestmentSettings.Dto;
using Cbms.Kms.Application.InvestmentSettings.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.InvestmentBranchSettings;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Mediator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.InvestmentSettings.CommandHandlers
{
    public class UpsertInvestmentSettingCommandHandler : CommandHandlerBase, IRequestHandler<UpsertInvestmentSettingCommand,InvestmentSettingDto>
    {
        private readonly IRepository<InvestmentSetting, int> _investmentSettingRepository;
        private readonly IRepository<InvestmentBranchSetting, int> _investmentBranchSettingRepository;

        public UpsertInvestmentSettingCommandHandler(IRequestSupplement supplement, IRepository<InvestmentSetting, int> investmentSettingRepository,
             IRepository<InvestmentBranchSetting, int> investmentBranchSettingRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _investmentSettingRepository = investmentSettingRepository;
            _investmentBranchSettingRepository = investmentBranchSettingRepository;
        }

        public async Task<InvestmentSettingDto> Handle(UpsertInvestmentSettingCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            InvestmentSetting entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _investmentSettingRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = InvestmentSetting.Create();
                await _investmentSettingRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new UpsertInvestmentSettingAction(
                entityDto.MaxInvestAmount,
                entityDto.AmountPerPoint,
                entityDto.MaxInvestmentQueryMonths,
                entityDto.CheckQrCodeBranch,
                entityDto.DefaultPointsForTicket,
                entityDto.BeginIssueDaysAfterCurrent,
                entityDto.EndIssueDaysBeforeOperation
            ));

            // loop  UpsertedItems va  DeletedItems 
            foreach(var branchSetting in entityDto.InvestmentBranchSettingChanges.UpsertedItems)
            {
                InvestmentBranchSetting entityBranch = null;
                if (!branchSetting.Id.IsNew())
                {
                    entityBranch = await _investmentBranchSettingRepository.GetAsync(branchSetting.Id);
                }
                if(entityBranch == null)
                {
                    entityBranch = InvestmentBranchSetting.Create();
                    await _investmentBranchSettingRepository.InsertAsync(entityBranch);
                }

                await entityBranch.ApplyActionAsync(new InvestmentBranchSettingUpsertAction(
                    branchSetting.BranchId,
                    true
                ));
                await _investmentBranchSettingRepository.UnitOfWork.CommitAsync();
            }
            foreach(var branchSetting in entityDto.InvestmentBranchSettingChanges.DeletedItems)
            {
                await Mediator.Send(new DeleteInvestmentBranchSettingCommand(branchSetting.Id));
            }            

            await _investmentSettingRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await Mediator.Send(new GetInvestmentSetting());
            //return Mapper.Map<InvestmentSettingDto>(await _investmentSettingRepository.FindAsync(entity.Id));
        }

    }
}
