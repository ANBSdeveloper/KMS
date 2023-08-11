using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.AppSettings.Commands;
using Cbms.Kms.Application.AppSettings.Dto;
using Cbms.Kms.Application.AppSettings.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/appsettings")]
    public class AppSettingController : AppController
    {
        public AppSettingController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetAppSetting")]
        [Produces(typeof(ApiResultObject<AppSettingDto>))]
        public async Task<AppSettingDto> Get(int id)
        {
            return await Mediator.Send(new GetAppSetting(id));
        }

        [HttpGet("shop", Name = "GetShopAppSettings")]
        [Produces(typeof(ApiResultObject<List<ShopAppSettingDto>>))]
        [AllowAnonymous]
        public async Task<List<ShopAppSettingDto>> GetShopAppSettingList()
        {
            return await Mediator.Send(new GetShopAppSettingList());
        }

        [HttpGet("sales", Name = "GetSalesAppSettings")]
        [Produces(typeof(ApiResultObject<List<SalesAppSettingDto>>))]
        [AllowAnonymous]
        public async Task<List<SalesAppSettingDto>> GetSalesAppSettingList()
        {
            return await Mediator.Send(new GetSalesAppSettingList());
        }

        [HttpGet(Name = "GetAppSettings")]
        [Produces(typeof(ApiResultObject<PagingResult<AppSettingDto>>))]
        public async Task<PagingResult<AppSettingDto>> GetList([FromQuery] GetAppSettingList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateAppSetting")]
        [Produces(typeof(ApiResultObject<AppSettingDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "AppSettings")]
        public async Task<AppSettingDto> Create([FromBody] UpsertAppSettingCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteAppSetting")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "AppSettings")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new DeleteAppSettingCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateAppSetting")]
        [Produces(typeof(ApiResultObject<AppSettingDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "AppSettings")]
        public async Task<AppSettingDto> Update([FromRoute] int id, [FromBody] UpsertAppSettingCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}
