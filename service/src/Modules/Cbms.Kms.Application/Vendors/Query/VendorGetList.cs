using Cbms.Kms.Application.Vendors.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Vendors.Query
{
    public class VendorGetList : EntityPagingResultQuery<VendorListDto>
    {
        public bool? IsActive { get; set; }
    }
}
