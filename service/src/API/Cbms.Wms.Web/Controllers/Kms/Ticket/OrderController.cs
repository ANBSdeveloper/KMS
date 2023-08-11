using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.CustomerSalesItems.Commands;
using Cbms.Kms.Application.Orders.Commands;
using Cbms.Kms.Application.Orders.Dto;
using Cbms.Kms.Application.Orders.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.Ticket
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/orders")]
    public class OrderController : AppController
    {
        public OrderController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetOrder")]
        [Produces(typeof(ApiResultObject<OrderDto>))]
        public async Task<OrderDto> Get(int id)
        {
            return await Mediator.Send(new OrderGet(id));
        }

        [HttpPost("", Name = "CreateOrder")]
        [Produces(typeof(ApiResultObject<OrderDto>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "Orders", "Orders.Create")]
        public async Task<OrderDto> CreateOrder([FromBody] OrderCreateCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpPost("add-sales-item", Name = "AddSalesItem")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Orders", "Orders.Create")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task<string> AddSalesItem([FromBody] CustomerSalesItemCreateCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet(Name = "GetOrders")]
        [Produces(typeof(ApiResultObject<PagingResult<OrderListItemDto>>))]
        public async Task<PagingResult<OrderListItemDto>> GetList([FromQuery] OrderGetList query)
        {
            var entity = await Mediator.Send(query);
            return entity;
        }

        [HttpGet("get-for-shop", Name = "GetOrdersForShop")]
        [Produces(typeof(ApiResultObject<List<OrderListItemDto>>))]
        public async Task<List<OrderListItemDto>> GetListForShop([FromQuery] OrderGetListForShop query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("getOrderDetails", Name = "GetOrderDetails")]
        [Produces(typeof(ApiResultObject<PagingResult<OrderDetailDto>>))]
        public async Task<PagingResult<OrderDetailDto>> GetOrderDetails([FromQuery] OrderDetailGetListByOrderId query)
        {
            var entity = await Mediator.Send(query);
            return entity;
        }

        [HttpPost("validate-spoon-code", Name = "ValidateSpoonCode")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ValidateSpoonCode([FromBody] OrderValidateSpoonCodeCommand command)
        {
            await Mediator.Send(command);
        }
    }
}