using Cbms.Application.Authentication.Permissions.Dto;
using Cbms.Application.Authentication.Permissions.Query;
using Cbms.Application.Authentication.Roles.Commands;
using Cbms.Application.Authentication.Roles.Dto;
using Cbms.Application.Authentication.Roles.Query;
using Cbms.Application.Authentication.UserResetTickets.CommandHandlers;
using Cbms.Application.Authentication.UserResetTickets.Commands;
using Cbms.Application.Authentication.Users.Commands;
using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.Application.Users.Commands;
using Cbms.Application.Users.Query;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Users.Commands;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;
using UserDto = Cbms.Application.Users.Dto.UserDto;
using UserListItemDto = Cbms.Application.Users.Dto.UserListItemDto;
namespace Cbms.Kms.Web.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/user-management")]
    public class UserManagementController : AppController
    {
        public UserManagementController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }
      
        [HttpGet("users/{id}", Name = "GetUser")]
        [Produces(typeof(ApiResultObject<UserDto>))]
        public async Task<UserDto> GetUser(int id)
        {
            return await Mediator.Send(new GetUserExt(id));
        }

        [HttpGet("users", Name = "GetUsers")]
        [Produces(typeof(ApiResultObject<PagingResult<UserListItemDto>>))]
        public async Task<PagingResult<UserListItemDto>> GetUserList([FromQuery] GetUserExtList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("customer-development-users", Name = "GetCustomerDevelopmentUsers")]
        [Produces(typeof(ApiResultObject<PagingResult<UserListItemDto>>))]
        public async Task<PagingResult<UserListItemDto>> GetCustomerDevelopmentUserList([FromQuery] GetUserCustomerDevelopmentList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("users", Name = "CreateUser")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Users")]
        [Produces(typeof(ApiResultObject<UserDto>))]
        public async Task<UserDto> CreateUser([FromBody] UpsertUserExtCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpPut("users/{id}", Name = "UpdateUser")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Users")]
        [Produces(typeof(ApiResultObject<UserDto>))]
        public async Task<UserDto> UpdateUser([FromRoute] int id, [FromBody] UpsertUserExtCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }

        [HttpPut("users/change-password", Name = "ChangeUserPassword")]
        public async Task ChangeUserPassword([FromBody] ChangeUserPasswordCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpDelete("users/{id}", Name = "DeleteUser")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Users")]
        public async Task DeleteUser(int id)
        {
            await Mediator.Send(new DeleteUserCommand(id));
        }

        [HttpPost("update-profile", Name = "UpdateProfile")]
        [Produces(typeof(ApiResultObject<UserDto>))]
        public async Task<UserDto> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("roles/{id}", Name = "GetRole")]
        [Produces(typeof(ApiResultObject<RoleDto>))]
        public async Task<RoleDto> GetRole(int id)
        {
            return await Mediator.Send(new GetRole(id));
        }

        [HttpDelete("roles/{id}", Name = "DeleteRole")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Roles")]
        public async Task DeleteRole(int id)
        {
            await Mediator.Send(new DeleteRoleCommand(id));
        }

        [HttpGet("roles", Name = "GetRoles")]
        [Produces(typeof(ApiResultObject<PagingResult<RoleListItemDto>>))]
        public async Task<PagingResult<RoleListItemDto>> GetRoleList([FromQuery] GetRoleList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("roles", Name = "CreateRole")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Roles")]
        [Produces(typeof(ApiResultObject<RoleDto>))]
        public async Task<RoleDto> CreateRole([FromBody] UpsertRoleCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpPut("roles/{id}", Name = "UpdateRole")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Roles")]
        [Produces(typeof(ApiResultObject<RoleDto>))]
        public async Task<RoleDto> UpdateRole([FromRoute] int id, [FromBody] UpsertRoleCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }

        [HttpGet("permissions/{id}", Name = "GetPermission")]
        [Produces(typeof(ApiResultObject<PermissionDto>))]
        public async Task<PermissionDto> GetPermission(int id)
        {
            return await Mediator.Send(new GetPermission(id));
        }

        [HttpGet("permissions", Name = "GetPermissions")]
        [Produces(typeof(ApiResultObject<PagingResult<PermissionDto>>))]
        public async Task<PagingResult<PermissionDto>> GetPermissionList([FromQuery] GetPermissionList query)
        {
            return await Mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpPost("send-email-reset-password")]
        public async Task SendEmailResetPassword([FromBody] SendEmailResetPasswordCommand command)
        {
            await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task SendEmailResetPassword([FromBody] ResetPasswordCommand command)
        {
            await Mediator.Send(command);
        }
    }
}