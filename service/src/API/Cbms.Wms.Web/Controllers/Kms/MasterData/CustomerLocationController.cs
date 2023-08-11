using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.CustomerLocations.Commands;
using Cbms.Kms.Application.CustomerLocations.Dto;
using Cbms.Kms.Application.CustomerLocations.Query;
using Cbms.Mediator.Query.Pagination;
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
    [Route("api/v1/customer-locations")]
    public class CustomerLocationController : AppController
    {
        public CustomerLocationController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetCustomerLocation")]
        [Produces(typeof(ApiResultObject<CustomerLocationDto>))]
        public async Task<CustomerLocationDto> Get(int id)
        {
            return await Mediator.Send(new CustomerLocationGet(id));
        }

        [HttpGet(Name = "GetCustomerLocations")]
        [Produces(typeof(ApiResultObject<PagingResult<CustomerLocationDto>>))]
        public async Task<PagingResult<CustomerLocationDto>> GetList([FromQuery] CustomerLocationGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateCustomerLocation")]
        [Produces(typeof(ApiResultObject<CustomerLocationDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "CustomerLocations")]
        public async Task<CustomerLocationDto> Create([FromBody] CustomerLocationUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteCustomerLocation")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "CustomerLocations")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new CustomerLocationDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateCustomerLocation")]
        [Produces(typeof(ApiResultObject<CustomerLocationDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "CustomerLocations")]
        public async Task<CustomerLocationDto> Update([FromRoute] int id, [FromBody] CustomerLocationUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}