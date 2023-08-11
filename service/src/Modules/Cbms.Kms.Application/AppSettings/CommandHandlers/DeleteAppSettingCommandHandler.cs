using Cbms.Kms.Application.AppSettings.Commands;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Mediator;

namespace Cbms.Kms.Application.AppSettings.CommandHandlers
{
    public class DeleteAppSettingCommandHandler : DeleteEntityCommandHandler<DeleteAppSettingCommand, AppSetting>
    {
        public DeleteAppSettingCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}