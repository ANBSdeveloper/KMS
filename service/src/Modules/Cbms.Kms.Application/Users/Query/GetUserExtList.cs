
using Cbms.Application.Users.Dto;
using Cbms.Mediator;

namespace Cbms.Application.Users.Query
{
    public class GetUserExtList : EntityPagingResultQuery<UserListItemDto>
    {
        public bool? IsActive { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
    }
}