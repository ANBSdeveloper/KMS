using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.ProductUnits.Commands;
using Cbms.Kms.Application.ProductUnits.Dto;
using Cbms.Kms.Application.ProductUnits.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.MasterData
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/productUnits")]
    public class ProductUnitController : AppController
    {
        public ProductUnitController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetProductUnit")]
        [Produces(typeof(ApiResultObject<ProductUnitDto>))]
        public async Task<ProductUnitDto> Get(int id)
        {
            return await Mediator.Send(new GetProductUnit(id));
        }

        [HttpGet(Name = "GetProductUnits")]
        [Produces(typeof(ApiResultObject<PagingResult<ProductUnitDto>>))]
        public async Task<PagingResult<ProductUnitDto>> GetList([FromQuery] GetProductUnitList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateProductUnit")]
        [Produces(typeof(ApiResultObject<ProductUnitDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "ProductUnits")]
        public async Task<ProductUnitDto> Create([FromBody] UpsertProductUnitCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteProductUnit")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "ProductUnits")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new DeleteProductUnitCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateProductUnit")]
        [Produces(typeof(ApiResultObject<ProductUnitDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "ProductUnits")]
        public async Task<ProductUnitDto> Update([FromRoute] int id, [FromBody] UpsertProductUnitCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}
