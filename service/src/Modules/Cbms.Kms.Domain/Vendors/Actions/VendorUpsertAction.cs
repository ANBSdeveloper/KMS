using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Vendors.Actions
{
    public class VendorUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public bool IsActive { get; private set; }
        public int? ZoneId { get; private set; }
        public string TaxReg { get; private set; }
        public string Representative { get; private set; }

        public VendorUpsertAction(string code, string name, string address, string phone, bool isActive, int? zoneId, string taxReg, string representative)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
            Address = address;
            Phone = phone;
            IsActive = IsActive;
            ZoneId = zoneId;
            TaxReg = taxReg;
            Representative = representative;
        }
    }
}
