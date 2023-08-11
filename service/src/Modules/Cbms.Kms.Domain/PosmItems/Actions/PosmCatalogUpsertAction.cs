using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.PosmItems.Actions
{
    public class PosmCatalogUpsertAction : IEntityAction
    {
        public int? Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Link { get; private set; }

        public PosmCatalogUpsertAction(int? id, string code, string name,string link)
        {
            Id = id;
            Code = code;
            Name = name;
            Link = link;
        }
    }
}
