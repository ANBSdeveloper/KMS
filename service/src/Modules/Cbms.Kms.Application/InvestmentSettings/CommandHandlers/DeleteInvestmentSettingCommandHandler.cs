using Cbms.Kms.Application.InvestmentSettings.Commands;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.InvestmentSettings.CommandHandlers
{
    public class DeleteInvestmentSettingCommandHandler : DeleteEntityCommandHandler<DeleteInvestmentSettingCommand, InvestmentSetting>
    {
        public DeleteInvestmentSettingCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}
