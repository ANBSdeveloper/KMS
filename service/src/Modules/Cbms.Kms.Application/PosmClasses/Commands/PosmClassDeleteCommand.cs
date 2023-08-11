using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmClasses.Commands
{
    public class PosmClassDeleteCommand : DeleteEntityCommand
    {
        public PosmClassDeleteCommand(int id) : base(id)
        {
        }
    }
}