using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Brands.Actions
{
    public class BrandUpsertAction : IEntityAction
    {
        public string Code { get; private set; }

        public string Name { get; private set; }

        public bool IsActive { get; private set; }

        public BrandUpsertAction(string code, string name, bool isActive)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
        }
    }
}