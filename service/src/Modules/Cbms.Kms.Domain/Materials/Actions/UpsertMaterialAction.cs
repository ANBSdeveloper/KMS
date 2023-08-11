using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Materials.Actions
{
    public class UpsertMaterialAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Value { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDesign { get; private set; }
        public int MaterialTypeId { get; private set; }

        public UpsertMaterialAction(string code, string name, int materialTypeId, string description, decimal value, bool isActive, bool isDesign)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
            Description = description;
            MaterialTypeId = materialTypeId;
            Value = value;
            IsActive = IsActive;
            IsDesign = isDesign;
        }
    }
}
