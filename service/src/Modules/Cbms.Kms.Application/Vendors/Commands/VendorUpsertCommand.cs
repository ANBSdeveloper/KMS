using Cbms.Kms.Application.Vendors.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Vendors.Commands
{
    public class VendorUpsertCommand : UpsertEntityCommand<VendorUpsertDto, VendorDto>
    {
        public VendorUpsertCommand(VendorUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}