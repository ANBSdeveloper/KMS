using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.ProductUnits.Actions
{
    public class UpsertProductUnitAction : IEntityAction
    {
        public string Code { get; private set; }

        public string Name { get; private set; }

        public bool IsActive { get; private set; }

        public UpsertProductUnitAction(string code, string name, bool isActive)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
        }
    }
}