using Cbms.Kms.Application.InvestmentSettings.Dto;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.InvestmentSettings.Commands
{
    public class UpsertInvestmentSettingCommand : UpsertEntityCommand<UpsertInvestmentSettingDto, InvestmentSettingDto>
    {
        public UpsertInvestmentSettingCommand(UpsertInvestmentSettingDto data, string handleType) : base(data, handleType)
        {
        }
    }
}
