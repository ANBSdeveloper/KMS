using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Geography.Provinces.Actions
{
    public class UpsertProvinceAction : IEntityAction
    {
        public string Code { get; private set; }

        public string Name { get; private set; }

        public UpsertProvinceAction(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}