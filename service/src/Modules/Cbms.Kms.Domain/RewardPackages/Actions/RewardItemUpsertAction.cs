using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.RewardPackages.Actions
{
    public class RewardItemUpsertAction : IEntityAction
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string DocumentLink { get; private set; }
        public int? ProductUnitId { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public int? ProductId { get; private set; }

        public RewardItemUpsertAction(
            int id,
            string code,
            string name,
            string documentLink,
            int? productunitId,
            decimal price,
            int quantity,
            int? productId)
        {
            Id = id;
            Code = code;
            Name = name;
            DocumentLink = documentLink;
            ProductUnitId = productunitId;
            Price = price;
            Quantity = quantity;
            ProductId = productId;
        }
    }
}