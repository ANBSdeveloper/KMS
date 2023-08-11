using AutoMapper;
using Cbms.Application.Authentication.Users.Dto;

namespace Cbms.Application.Users.Dto
{
    public class UserListItemDto : UserBaseDto
    {
        public string RoleName { get; set; }
    }
}