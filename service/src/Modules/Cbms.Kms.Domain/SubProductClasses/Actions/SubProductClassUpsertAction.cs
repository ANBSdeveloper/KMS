using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.SubProductClasses.Actions
{
    public class SubProductClassUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public SubProductClassUpsertAction(string code, string name, bool isActive)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
        }
    }
}