using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.MaterialTypes.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.MaterialTypes
{
    public class MaterialType : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        private MaterialType()
        {
        }

        public static MaterialType Create()
        {
            return new MaterialType();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case MaterialTypeUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(MaterialTypeUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
        }
    }
}
