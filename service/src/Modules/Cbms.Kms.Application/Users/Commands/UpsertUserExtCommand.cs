using Cbms.Application.Users.Dto;
using Cbms.Mediator;

namespace Cbms.Application.Users.Commands
{
    public class UpsertUserExtCommand : UpsertEntityCommand<UpsertUserDto, UserDto>
    {
        public UpsertUserExtCommand(UpsertUserDto data, string handleType) : base(data, handleType)
        {
        }
    }
}