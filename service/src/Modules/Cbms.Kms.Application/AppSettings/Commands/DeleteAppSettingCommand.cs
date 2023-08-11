using Cbms.Mediator;

namespace Cbms.Kms.Application.AppSettings.Commands
{
    public class DeleteAppSettingCommand : DeleteEntityCommand
    {
        public DeleteAppSettingCommand(int id) : base(id)
        {
        }
    }
}