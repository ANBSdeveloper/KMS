using Cbms.Mediator;

namespace Cbms.Kms.Application.CustomerLocations.Commands
{
    public class CustomerLocationDeleteCommand : DeleteEntityCommand
    {
        public CustomerLocationDeleteCommand(int id) : base(id)
        {
        }
    }
}