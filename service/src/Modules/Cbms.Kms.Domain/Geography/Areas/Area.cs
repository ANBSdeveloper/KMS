using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Areas.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Areas
{
    public class Area : AuditedAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public int ZoneId { get; private set; }
        public int SalesOrgId { get; private set; }
        public Area()
        {
        }

        public static Area Create()
        {
            return new Area();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertAreaAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertAreaAction action)
        {
            Code = action.Code;
            Name = action.Name;
            ZoneId = action.ZoneId;
            SalesOrgId = action.SalesOrgId;
        }
    }
}