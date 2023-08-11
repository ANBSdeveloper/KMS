using Cbms.Kms.Application.AppSettings.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.AppSettings.Commands
{
    public class UpsertAppSettingCommand : UpsertEntityCommand<UpsertAppSettingDto, AppSettingDto>
    {
        public UpsertAppSettingCommand(UpsertAppSettingDto data, string handleType) : base(data, handleType)
        {
        }
    }
}