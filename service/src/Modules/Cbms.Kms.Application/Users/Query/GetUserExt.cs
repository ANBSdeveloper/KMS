using Cbms.Application.Users.Dto;
using Cbms.Mediator;

namespace Cbms.Application.Users.Query
{
    public class GetUserExt : EntityQuery<UserDto>
    {
        public GetUserExt(int id) : base(id)
        {
        }
    }
}