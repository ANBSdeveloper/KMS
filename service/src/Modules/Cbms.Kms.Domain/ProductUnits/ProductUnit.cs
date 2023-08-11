using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.ProductUnits.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.ProductUnits
{
    public class ProductUnit : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public ProductUnit()
        {
        }

        public static ProductUnit Create()
        {
            return new ProductUnit();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertProductUnitAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertProductUnitAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
        }
    }
}