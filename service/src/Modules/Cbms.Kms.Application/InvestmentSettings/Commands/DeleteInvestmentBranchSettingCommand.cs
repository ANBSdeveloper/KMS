using Cbms.Mediator;

namespace Cbms.Kms.Application.InvestmentSettings.Commands
{
    public class DeleteInvestmentBranchSettingCommand : DeleteEntityCommand
    {
        public DeleteInvestmentBranchSettingCommand(int id) : base(id)
        {
        }
    }
}
