using Cbms.Mediator;

namespace Cbms.Kms.Application.InvestmentSettings.Commands
{
    public class DeleteInvestmentSettingCommand : DeleteEntityCommand
    {
        public DeleteInvestmentSettingCommand(int id) : base(id)
        {
        }
    }
}
