using AutoMapper;
using Cbms.Kms.Application.InvestmentSettings.Dto;
using Cbms.Kms.Domain.InvestmentSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.InvestmentSettings
{
    public class InvestmentSettingProfile : Profile
    {
        public InvestmentSettingProfile()
        {
            CreateMap<InvestmentSettingDto, UpsertInvestmentSettingAction>();
        }
    }
}
