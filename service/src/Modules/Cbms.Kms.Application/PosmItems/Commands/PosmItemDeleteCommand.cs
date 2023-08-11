using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmItems.Commands
{
    public class PosmItemDeleteCommand : DeleteEntityCommand
    {
        public PosmItemDeleteCommand(int id) : base(id)
        {
        }
    }
}