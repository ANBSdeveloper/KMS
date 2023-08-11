using Cbms.Mediator;

namespace Cbms.Kms.Application.Cycles.Commands
{
    public class DeleteCycleCommand : DeleteEntityCommand
    {
        public DeleteCycleCommand(int id) : base(id)
        {
        }
    }
}