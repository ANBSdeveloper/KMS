using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.Authorization;
using Cbms.Mediator.Query.Pagination;
using Cbms.Kms.Application.ProductClasses.Commands;
using Cbms.Kms.Application.ProductClasses.Dto;
using Cbms.Kms.Application.ProductClasses.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;
using Cbms.AspNetCore.Web.Models;

namespace Cbms.Kms.Web.Controllers.Kms.MasterData
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/product-classes")]
    public class ProductClassController : AppController
    {
        public ProductClassController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetProductClass")]
        [Produces(typeof(ApiResultObject<ProductClassDto>))]
        public async Task<ProductClassDto> Get(int id)
        {
            return await Mediator.Send(new GetProductClass(id));
        }

        [HttpGet(Name = "GetProductClasses")]
        [Produces(typeof(ApiResultObject<PagingResult<ProductClassDto>>))]
        public async Task<PagingResult<ProductClassDto>> GetList([FromQuery] GetProductClassList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateProductClass")]
        [Produces(typeof(ApiResultObject<ProductClassDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "ProductClasses")]
        public async Task<ProductClassDto> Create([FromBody] UpsertProductClassCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteProductClass")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "ProductClasses")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new DeleteProductClassCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateProductClass")]
        [Produces(typeof(ApiResultObject<ProductClassDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "ProductClasses")]
        public async Task<ProductClassDto> Update([FromRoute] int id, [FromBody] UpsertProductClassCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}