using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmTypes.Commands
{
    public class PosmTypeDeleteCommand : DeleteEntityCommand
    {
        public PosmTypeDeleteCommand(int id) : base(id)
        {
        }
    }
}