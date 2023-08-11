using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.CustomerLocations.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.CustomerLocations
{
    public class CustomerLocation : AuditedAggregateRoot
    {
     
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public CustomerLocation()
        {
        }
        public static CustomerLocation Create()
        {
            return new CustomerLocation();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case CustomerLocationUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(CustomerLocationUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
        }
    }
}
