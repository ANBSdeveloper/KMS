using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmPrices.Commands
{
    public class PosmPriceHeaderDeleteCommand : DeleteEntityCommand
    {
        public PosmPriceHeaderDeleteCommand(int id) : base(id)
        {
        }
    }
}