using Cbms.Mediator;

namespace Cbms.Kms.Application.Vendors.Commands
{
    public class VendorDeleteCommand : DeleteEntityCommand
    {
        public VendorDeleteCommand(int id) : base(id)
        {
        }
    }
}