using Cbms.Mediator;

namespace Cbms.Kms.Application.Users.Commands
{
    public class DeleteUserCommand : DeleteEntityCommand
    {
        public DeleteUserCommand(int id) : base(id)
        {
        }
    }
}