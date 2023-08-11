using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.RewardPackages.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.RewardPackages
{
    public class RewardItem : AuditedEntity
    {
        public int RewardPackageId { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string DocumentLink { get; private set; }
        public int? ProductUnitId { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public int? ProductId { get; private set; }

        private RewardItem()
        {
        }

        public static RewardItem Create()
        {
            return new RewardItem();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case RewardItemUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(RewardItemUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            DocumentLink = action.DocumentLink;
            ProductUnitId = action.ProductUnitId;
            Price = action.Price;
            Quantity = action.Quantity;
            ProductId = action.ProductId;
        }
    }
}