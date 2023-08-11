using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Areas.Actions
{
    public class UpsertAreaAction : IEntityAction
    {
        public string Code { get; private set; }

        public string Name { get; private set; }
        public int ZoneId { get; private set; }
        public int SalesOrgId { get; private set; }
        public UpsertAreaAction(string code, string name, int zoneId, int salesOrgId)
        {
            Code = code;
            Name = name;
            ZoneId = zoneId;
            SalesOrgId = salesOrgId;
        }
    }
}