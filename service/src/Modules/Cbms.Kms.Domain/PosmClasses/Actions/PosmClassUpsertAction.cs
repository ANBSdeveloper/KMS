using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.PosmClasses.Actions
{
    public class PosmClassUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IncludeInfo { get; private set; }

        public PosmClassUpsertAction(string code, string name, bool includeInfo, bool isActive)
        {
            Code = code;
            Name = name;
            IncludeInfo = includeInfo;
            IsActive = isActive;
        }
    }
}