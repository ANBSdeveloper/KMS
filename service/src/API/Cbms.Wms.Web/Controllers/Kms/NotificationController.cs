using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Notifications.Commands;
using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Kms.Application.Notifications.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/notifications")]
    public class NotificationController : AppController
    {
        public NotificationController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetNotification")]
        [Produces(typeof(ApiResultObject<NotificationDto>))]
        public async Task<NotificationDto> Get(int id)
        {
            return await Mediator.Send(new NotificationGet(id));
        }

        [HttpGet(Name = "GetNotifications")]
        [Produces(typeof(ApiResultObject<PagingResult<NotificationListDto>>))]
        public async Task<PagingResult<NotificationListDto>> GetList([FromQuery] NotificationGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateNotification")]
        [Produces(typeof(ApiResultObject<NotificationDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Notifications")]
        public async Task<NotificationDto> Create([FromBody] NotificationUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpPost("{id}/send", Name = "SendNotification")]
        [Produces(typeof(ApiResultObject<NotificationDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Notifications")]
        public async Task<NotificationDto> Send(int id)
        {
            var entityDto = await Mediator.Send(new NotificationSendCommand(new NotificationSendDto() { Id = id}, ""));
            return entityDto;
        }

        [HttpPost("{id}/view", Name = "ViewNotification")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task View(int id)
        {
            await Mediator.Send(new NotificationViewCommand(id));
        }

        [HttpDelete("{id}", Name = "DeleteNotification")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Notifications")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new NotificationDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateNotification")]
        [Produces(typeof(ApiResultObject<NotificationDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Notifications")]
        public async Task<NotificationDto> Update([FromRoute] int id, [FromBody] NotificationUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
       
        [HttpGet("byuser", Name = "GetNotificationUsers")]
        [Produces(typeof(ApiResultObject<PagingResult<NotificationListDto>>))]
        public async Task<PagingResult<NotificationUserListDto>> GetUserList([FromQuery] NotificationGetUserList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("unread-count", Name = "GetNotificationUnreadCount")]
        [Produces(typeof(ApiResultObject<int>))]
        public async Task<int> GetUnreadCount()
        {
            return await Mediator.Send(new NotificationUnreadCountGet());
        }
    }
}