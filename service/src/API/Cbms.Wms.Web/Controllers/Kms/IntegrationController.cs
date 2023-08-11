using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Orders.Commands;
using Cbms.Kms.Application.Orders.Dto;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Application.Vendors.Commands;
using Cbms.Kms.Application.Vendors.Dto;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Cbms.Kms.Domain.Orders.Actions.OrderUpdateSpoonCodeAction;

namespace Cbms.Kms.Web.Controllers.Kms
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "integration")]
    [Route("api/v1/integration")]
    public class IntegrationController : AppController
    {
        public IntegrationController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }
        
        [HttpPost("update-spoon-code", Name = "UpdateSpoonCode")]
        [Produces(typeof(ApiResultObject<OrderDto>))]
        public async Task<OrderUpdateSpoonResult> UpdateSpoon([FromBody] OrderUpdateSpoonCodeCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpGet("tickets", Name = "Tickets")]
        [Produces(typeof(ApiResultObject<List<TicketGetByConsumerDto>>))]
        public async Task<List<TicketGetByConsumerDto>> GetTickets(string phone)
        {
            return await Mediator.Send(new TicketGetByConsumer() { Phone = phone});
        }


        [HttpPost("vendors", Name = "IntegrationCreateVendor")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task<VendorDto> IntegrationCreateVendor([FromBody] VendorUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpPut("vendors/{id}", Name = "IntegrationUpdateVendor")]
        [Produces(typeof(ApiResultObject<VendorDto>))]
        public async Task<VendorDto> IntegrationUpdateVendor([FromRoute] int id, [FromBody] VendorUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}