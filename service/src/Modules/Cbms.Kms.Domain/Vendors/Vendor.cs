using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Vendors.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Vendors
{
    public class Vendor : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public bool IsActive { get; private set; }
        public string TaxReg { get;  private set; }
        public int? ZoneId { get; private set; }
        public string Representative { get; private set; }
        public Vendor()
        {
        }
        public static Vendor Create()
        {
            return new Vendor();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case VendorUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(VendorUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
            Address = action.Address;
            Phone = action.Phone;
            TaxReg = action.TaxReg;
            Representative = action.Representative;
            ZoneId = action.ZoneId;
        }
    }
}
