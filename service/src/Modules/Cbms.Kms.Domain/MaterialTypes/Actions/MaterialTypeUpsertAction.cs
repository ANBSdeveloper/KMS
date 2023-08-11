using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.MaterialTypes.Actions
{
    public class MaterialTypeUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public MaterialTypeUpsertAction(string code, string name, bool isActive)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
        }
    }
}
