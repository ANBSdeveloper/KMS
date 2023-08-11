using Cbms.Kms.Application.InvestmentSettings.Commands;
using Cbms.Kms.Domain.InvestmentBranchSettings;
using Cbms.Mediator;

namespace Cbms.Kms.Application.InvestmentSettings.CommandHandlers
{
    public class DeleteInvestmentBranchSettingCommandHandler : DeleteEntityCommandHandler<DeleteInvestmentBranchSettingCommand, InvestmentBranchSetting>
    {
        public DeleteInvestmentBranchSettingCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}
