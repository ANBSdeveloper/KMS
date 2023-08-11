using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.InvestmentSettings.Commands;
using Cbms.Kms.Application.InvestmentSettings.Dto;
using Cbms.Kms.Application.InvestmentSettings.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.Ticket
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/investmentSetting")]
    public class InvestmentSettingsController : AppController
    {
        public InvestmentSettingsController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet(Name = "GetInvestmentSetting")]
        [Produces(typeof(ApiResultObject<InvestmentSettingDto>))]
        public async Task<InvestmentSettingDto> Get()
        {
            return await Mediator.Send(new GetInvestmentSetting());
        }

        [HttpGet("customer", Name = "GetInvestmentCustomerSetting")]
        [Produces(typeof(ApiResultObject<InvestmentCustomerSettingDto>))]
        public async Task<InvestmentCustomerSettingDto> GetCustomerSettingById([FromQuery] GetInvestmentCustomerSettingById query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("customer/bycode", Name = "GetInvestmentCustomerSettingByCode")]
        [Produces(typeof(ApiResultObject<InvestmentCustomerSettingDto>))]
        public async Task<InvestmentCustomerSettingDto> GetCustomerSettingByCode([FromQuery] GetInvestmentCustomerSettingByCode query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateInvestmentSetting")]
        [Produces(typeof(ApiResultObject<InvestmentSettingDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "InvestmentSettings")]
        public async Task<InvestmentSettingDto> Create([FromBody] UpsertInvestmentSettingCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteInvestmentSetting")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "InvestmentSettings")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new DeleteInvestmentSettingCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateInvestmentSetting")]
        [Produces(typeof(ApiResultObject<InvestmentSettingDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "InvestmentSettings")]
        public async Task<InvestmentSettingDto> Update([FromRoute] int id, [FromBody] UpsertInvestmentSettingCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}