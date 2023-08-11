using Cbms.Kms.Application.Vendors.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Vendors.Query
{
    public class VendorGet : EntityQuery<VendorDto>
    {
        public VendorGet(int id) : base(id)
        {
        }
    }
}
