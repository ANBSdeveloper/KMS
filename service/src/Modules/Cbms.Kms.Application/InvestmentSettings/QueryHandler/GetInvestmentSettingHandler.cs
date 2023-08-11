using Cbms.Domain.Repositories;
using Cbms.Kms.Application.InvestmentSettings.Dto;
using Cbms.Kms.Application.InvestmentSettings.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.InvestmentSettings.QueryHandler
{
    public class GetInvestmentSettingHandler : QueryHandlerBase, IRequestHandler<GetInvestmentSetting, InvestmentSettingDto>
    {
        private readonly IRepository<InvestmentSetting, int> _repository;
        private readonly AppDbContext _dbContext;

        public GetInvestmentSettingHandler(IRequestSupplement supplement, IRepository<InvestmentSetting, int> repository, AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<InvestmentSettingDto> Handle(GetInvestmentSetting request, CancellationToken cancellationToken)
        {
            var rewardBranchDtos = await (
                                        from branch in _dbContext.Branches
                                        join investmentSettingBranch in _dbContext.InvestmentBranchSettings on new { BranchId = branch.Id} equals new { BranchId = investmentSettingBranch.BranchId} into investmentSettingBranchLeft
                                        from investmentSettingBranch in investmentSettingBranchLeft.DefaultIfEmpty()
                                        join area in _dbContext.Areas on branch.AreaId equals area.Id
                                        join zone in _dbContext.Zones on branch.ZoneId equals zone.Id
                                        where branch.IsActive
                                        select new InvestmentBranchSettingDto()
                                        {
                                            Id = investmentSettingBranch.Id,
                                            BranchId = branch.Id,
                                            BranchCode = branch.Code,
                                            BranchName = branch.Name,
                                            IsEditablePoint = investmentSettingBranch.IsEditablePoint,
                                            IsSelected = investmentSettingBranch != null,
                                            AreaId = branch.AreaId,
                                            AreaName = area.Name,
                                            ZoneId = branch.ZoneId,
                                            ZoneName = zone.Name
                                        }).ToListAsync();
            var entityDto = Mapper.Map<InvestmentSettingDto>(_repository.GetAll().FirstOrDefault());
            entityDto.InvestmentSettingBranchs = rewardBranchDtos;
            return entityDto;
        }
    }
}
