using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Cbms.Dto;
using Cbms.Kms.Domain.InvestmentSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.InvestmentSettings.Dto
{
    [AutoMap(typeof(InvestmentSetting))]
    public class InvestmentSettingDto : InvestmentSettingBaseDto
    {
        [Ignore]
        public List<InvestmentBranchSettingDto> InvestmentSettingBranchs { get; set; }
    }
}
