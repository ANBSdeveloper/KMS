using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Cbms.Application.Authentication.Users.Dto;
using Cbms.Authorization.Users;
using System.Collections.Generic;

namespace Cbms.Application.Users.Dto
{
    [AutoMap(typeof(User))]
    public class UserDto : UserBaseDto
    {
        public string RoleName { get; set; }
        [Ignore]
        public List<UserRoleDto> Roles { get; set; }
        [Ignore]
        public List<UserAssignmentDto> Assignments { get; set; }
    }
}