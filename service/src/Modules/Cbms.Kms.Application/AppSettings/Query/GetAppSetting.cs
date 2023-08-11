using Cbms.Kms.Application.AppSettings.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.AppSettings.Query
{
    public class GetAppSetting : EntityQuery<AppSettingDto>
    {
        public GetAppSetting(int id) : base(id)
        {
        }
    }
}