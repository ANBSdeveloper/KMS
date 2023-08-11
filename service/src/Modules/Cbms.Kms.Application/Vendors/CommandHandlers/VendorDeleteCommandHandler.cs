using Cbms.Kms.Application.Vendors.Commands;
using Cbms.Kms.Domain.Vendors;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Vendors.CommandHandlers
{
    public class VendorDeleteCommandHandler : DeleteEntityCommandHandler<VendorDeleteCommand, Vendor>
    {
        public VendorDeleteCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}