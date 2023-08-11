using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Geography.Districts.Actions
{
    public class UpsertDistrictAction : IEntityAction
    {
        public string Code { get; private set; }

        public string Name { get; private set; }
        public int ProvinceId { get; private set; }

        public UpsertDistrictAction(string code, string name, int province)
        {
            Code = code;
            Name = name;
            ProvinceId = province;
        }
    }
}