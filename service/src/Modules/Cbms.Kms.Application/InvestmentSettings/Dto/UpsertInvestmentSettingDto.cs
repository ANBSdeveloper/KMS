using Cbms.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.InvestmentSettings.Dto
{
    public class UpsertInvestmentSettingDto : InvestmentSettingBaseDto
    {

        public CrudListDto<UpsertInvestmentBranchSettingDto> InvestmentBranchSettingChanges { get; set; }

        public UpsertInvestmentSettingDto()
        {
            InvestmentBranchSettingChanges = new CrudListDto<UpsertInvestmentBranchSettingDto>();
        }
    }
}
