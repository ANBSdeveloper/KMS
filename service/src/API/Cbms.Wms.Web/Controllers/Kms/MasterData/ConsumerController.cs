using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Kms.Application.Consumers.Commands;
using Cbms.Kms.Application.Consumers.Dto;
using Cbms.Kms.Application.Customers.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.MasterData
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/consumers")]
    public class ConsumerController : AppController
    {
        public ConsumerController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("find", Name = "FindConsumerByPhone")]
        [Produces(typeof(ApiResultObject<ConsumerInfoDto>))]
        public async Task<ConsumerInfoDto> FindByPhone([FromQuery] ConsumerGetByPhone query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("send-otp", Name = "SendOtpConsumer")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task SendOtp([FromBody] ConsumerSendOtpCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("validate-otp", Name = "ValidateOtpConsumer")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ValidateOtp([FromBody] ConsumerValidateOtpCommand command)
        {
            await Mediator.Send(command);
        }
    }
}