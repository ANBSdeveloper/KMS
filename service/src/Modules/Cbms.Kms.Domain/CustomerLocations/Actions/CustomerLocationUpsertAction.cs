using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.CustomerLocations.Actions
{
    public class CustomerLocationUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public CustomerLocationUpsertAction(string code, string name, bool isActive)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
        }
    }
}
