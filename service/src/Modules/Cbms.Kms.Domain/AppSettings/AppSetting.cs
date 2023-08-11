using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.AppSettings.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.AppSettings
{
    public class AppSetting : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Value { get; private set; }
        public string Description { get; private set; }
        public AppSetting()
        {
        }

        public static AppSetting Create()
        {
            return new AppSetting();
        }
        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case AppSettingUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(AppSettingUpsertAction action)
        {
            Code = action.Code;
            Value = action.Value;
            Description = action.Description;
        }
    }
}
