using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Entities;
using Cbms.Kms.Domain.PosmItems.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cbms.Kms.Domain.PosmItems;
using System.Linq;

namespace Cbms.Kms.Domain.PosmItems
{
    public class PosmItem : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int PosmClassId { get; private set; }
        public bool IsActive { get; private set; }
        public string Link { get; private set; }
        public int? PosmTypeId { get; private set; }
        public PosmUnitType UnitType { get; set; }
        public PosmCalcType CalcType { get; set; }

        public List<PosmCatalog> _posmCatalogs = new List<PosmCatalog>();
        public IReadOnlyCollection<PosmCatalog> PosmCatalogs => _posmCatalogs;
        private PosmItem()
        {
        }

        public static PosmItem Create()
        {
            return new PosmItem()
            {
            };
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmItemUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(PosmItemUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
            PosmClassId = action.PosmClassId;
            PosmTypeId = action.PosmTypeId;
            UnitType = action.UnitType;
            CalcType = action.CalcType;
            Link = action.Link;
            
            foreach (var id in action.DeleteCatalogs)
            {
                var catalog = _posmCatalogs.FirstOrDefault(p => p.Id == id);
                if (catalog != null)
                {
                    _posmCatalogs.Remove(catalog);
                }
            }

            foreach (var item in action.UpsertCatalogs)
            {
                PosmCatalog catalog = null;

                if (item.Id.HasValue && item.Id != 0)
                {
                    catalog = _posmCatalogs.FirstOrDefault(p => p.Id == item.Id);
                    if (catalog == null)
                    {
                        throw new EntityNotFoundException(typeof(PosmCatalog), item.Id);
                    }
                }
                else
                {
                    catalog = _posmCatalogs.FirstOrDefault(p => p.Code == item.Code);
                    if (catalog == null)
                    {
                        catalog = PosmCatalog.Create();
                        _posmCatalogs.Add(catalog);
                    }
                }

                await catalog.ApplyActionAsync(item);
            }
        }
    }
}
