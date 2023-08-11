using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Brands.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Brands
{
    public class Brand : AuditedAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public bool IsActive { get; private set; }

        private Brand()
        {
        }

        public static Brand Create()
        {
            return new Brand();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case BrandUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(BrandUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
        }
    }
}