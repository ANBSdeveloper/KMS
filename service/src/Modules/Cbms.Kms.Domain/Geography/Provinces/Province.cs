using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Geography.Provinces.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Geography.Provinces
{
    public class Province : AuditedAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }

        public Province()
        {
        }

        public static Province Create()
        {
            return new Province();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertProvinceAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertProvinceAction action)
        {
            Code = action.Code;
            Name = action.Name;
        }
    }
}