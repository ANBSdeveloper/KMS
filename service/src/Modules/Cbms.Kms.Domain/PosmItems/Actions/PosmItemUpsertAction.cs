using Cbms.Domain.Entities;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.PosmItems.Actions
{
    public class PosmItemUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int PosmClassId { get; private set; }
        public int? PosmTypeId { get; private set; }
        public bool IsActive { get; private set; }
        public string Link { get; private set; }
        public PosmUnitType UnitType { get; set; }
        public PosmCalcType CalcType { get; set; }
        public List<PosmCatalogUpsertAction> UpsertCatalogs { get; set; }
        public List<int> DeleteCatalogs { get; set; } 
        public PosmItemUpsertAction(
            string code,
            string name,
            int posmClassId,
            int? posmTypeId,
            bool isActive,
            string link,
            PosmUnitType unitType,
            PosmCalcType calcType,
            List<PosmCatalogUpsertAction> upsertCatalogs,
            List<int> deleteCatalogs)
        {
            Code= code;
            Name= name;
            PosmClassId= posmClassId;
            PosmTypeId= posmTypeId;
            Link = link;
            IsActive= isActive;
            UnitType= unitType;
            CalcType= calcType;
            UpsertCatalogs= upsertCatalogs;
            DeleteCatalogs= deleteCatalogs;
        }
    }
}