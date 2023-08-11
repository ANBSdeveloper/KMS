using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Materials.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Materials
{
    public class Material : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Value { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDesign { get; private set; }
        public int MaterialTypeId { get; private set; }
        public Material()
        {
        }
        public static Material Create()
        {
            return new Material();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertMaterialAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertMaterialAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
            Description = action.Description;
            Value = action.Value;
            IsDesign = action.IsDesign;
            MaterialTypeId = action.MaterialTypeId;
        }
    }
}
