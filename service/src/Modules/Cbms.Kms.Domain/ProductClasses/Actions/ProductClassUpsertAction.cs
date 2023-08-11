using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.ProductClasses.Actions
{
    public class ProductClassUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string RewardCode { get; private set; }
        public bool IsActive { get; private set; }

        public ProductClassUpsertAction(string code, string name, string rewardCode, bool isActive)
        {
            Code = code;
            Name = name;
            RewardCode = rewardCode;
            IsActive = isActive;
        }
    }
}