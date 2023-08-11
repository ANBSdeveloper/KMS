using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.ProductClasses.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.ProductClasses
{
    public class ProductClass : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string RewardCode { get; private set; }
        public bool IsActive { get; private set; }

        private ProductClass()
        {
        }

        public static ProductClass Create()
        {
            return new ProductClass()
            {
            };
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case ProductClassUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(ProductClassUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            RewardCode = action.RewardCode;
            IsActive = action.IsActive;
        }
    }
}