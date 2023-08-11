using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Geography.Districts.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Geography.Districts
{
    public class District : AuditedAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public int ProvinceId { get; private set; }

        private District()
        {
        }

        public static District Create()
        {
            return new District();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertDistrictAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertDistrictAction action)
        {
            Code = action.Code;
            Name = action.Name;
            ProvinceId = action.ProvinceId;
        }
    }
}