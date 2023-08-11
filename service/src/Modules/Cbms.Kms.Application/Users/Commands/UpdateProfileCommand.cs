using Cbms.Application.Users.Dto;
using Cbms.Mediator;

namespace Cbms.Application.Users.Commands
{
    public class UpdateProfileCommand : CommandBase<UserDto>
    {
        public UpdateProfileDto Data { get; set; }
    }
}