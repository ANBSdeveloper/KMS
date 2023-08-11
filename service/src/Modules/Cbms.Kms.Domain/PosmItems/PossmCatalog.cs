using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using System.Threading.Tasks;
using Cbms.Kms.Domain.PosmItems.Actions;

namespace Cbms.Kms.Domain.PosmItems
{
    public class PosmCatalog: AuditedEntity
    {
        public int PosmItemId { get; private set; }
        public string Code  { get; private set; }
        public string Name { get; private set; }
        public string Link { get; private set; }


        public static PosmCatalog Create()
        {
            return new PosmCatalog();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmCatalogUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(PosmCatalogUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            Link = action.Link;
        }
    }
}
