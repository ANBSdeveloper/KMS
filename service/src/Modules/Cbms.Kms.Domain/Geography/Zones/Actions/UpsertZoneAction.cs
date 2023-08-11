using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Zones.Actions
{
    public class UpsertZoneAction : IEntityAction
    {
        public string Code { get; private set; }

        public string Name { get; private set; }
        public int SalesOrgId { get; private set; }
        public UpsertZoneAction(string code, string name, int salesOrgId)
        {
            Code = code;
            Name = name;
            SalesOrgId = salesOrgId;
        }
    }
}