using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Zones.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Zones
{
    public class Zone : AuditedAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public int SalesOrgId { get; private set; }
        public Zone()
        {
        }

        public static Zone Create()
        {
            return new Zone();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertZoneAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertZoneAction action)
        {
            Code = action.Code;
            Name = action.Name;
            SalesOrgId = action.SalesOrgId;
        }
    }
}