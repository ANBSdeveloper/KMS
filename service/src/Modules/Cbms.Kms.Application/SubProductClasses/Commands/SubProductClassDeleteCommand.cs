using Cbms.Mediator;

namespace Cbms.Kms.Application.SubProductClasses.Commands
{
    public class SubProductClassDeleteCommand : DeleteEntityCommand
    {
        public SubProductClassDeleteCommand(int id) : base(id)
        {
        }
    }
}