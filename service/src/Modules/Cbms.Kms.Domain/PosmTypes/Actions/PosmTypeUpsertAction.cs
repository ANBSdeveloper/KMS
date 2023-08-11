using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.PosmTypes.Actions
{
    public class PosmTypeUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public PosmTypeUpsertAction(string code, string name, bool isActive)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
        }
    }
}