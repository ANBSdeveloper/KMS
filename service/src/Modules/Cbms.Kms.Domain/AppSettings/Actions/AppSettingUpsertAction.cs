using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.AppSettings.Actions
{
    public class AppSettingUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Value { get; private set; }
        public string Description { get; private set; }

        public AppSettingUpsertAction(string code, string data, string description)
        {
            Code = code;
            Value = data;
            Description = description;
        }
    }
}