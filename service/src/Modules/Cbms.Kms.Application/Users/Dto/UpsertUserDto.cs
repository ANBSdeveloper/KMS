using Cbms.Application.Authentication.Users.Dto;
using Cbms.Dto;

namespace Cbms.Application.Users.Dto
{
    public class UpsertUserDto : UserDto
    {
        public string Password { get; set; }
        public CrudListDto<UserRoleDto> RoleChanges { get; set; }
        public CrudListDto<UpsertUserAssignmentDto> AssignmentChanges { get; set; }
        public UpsertUserDto()
        {
            RoleChanges = new CrudListDto<UserRoleDto>();
            AssignmentChanges = new CrudListDto<UpsertUserAssignmentDto>();
        }
    }
}