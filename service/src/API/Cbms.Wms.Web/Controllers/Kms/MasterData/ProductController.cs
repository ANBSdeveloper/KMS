using Cbms.Application.Runtime;
using Cbms.Mediator.Query.Pagination;
using Cbms.Kms.Application.Products.Dto;
using Cbms.Kms.Application.Products.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;
using Cbms.AspNetCore.Web.Models;
using Cbms.Application.Authorization;
using Cbms.Authorization;
using Cbms.Kms.Application.Products.Commands;

namespace Cbms.Kms.Web.Controllers.Kms.MasterData
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/products")]
    public class ProductController : AppController
    {
        public ProductController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [Produces(typeof(ApiResultObject<ProductBaseDto>))]
        public async Task<ProductBaseDto> Get(int id)
        {
            return await Mediator.Send(new GetProduct(id));
        }

        [HttpGet("item/{id}", Name = "GetProductItem")]
        [Produces(typeof(ApiResultObject<ProductItemDto>))]
        public async Task<ProductItemDto> GetItem(int id)
        {
            return await Mediator.Send(new GetProductItem(id));
        }

        [HttpGet(Name = "GetProducts")]
        [Produces(typeof(ApiResultObject<PagingResult<ProductListItemDto>>))]
        public async Task<PagingResult<ProductListItemDto>> GetList([FromQuery] GetProductList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("itembyclass", Name = "GetProductItemClasses")]
        [Produces(typeof(ApiResultObject<PagingResult<ProductListItemDto>>))]
        public async Task<PagingResult<ProductListItemDto>> GetListItemClass([FromQuery] GetProductListByClass query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateProduct")]
        [Produces(typeof(ApiResultObject<ProductBaseDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Products")]
        public async Task<ProductBaseDto> Create([FromBody] UpsertProductCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }
        [HttpPut("{id}", Name = "UpdateProduct")]
        [Produces(typeof(ApiResultObject<ProductBaseDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Products")]
        public async Task<ProductBaseDto> Update([FromRoute] int id, [FromBody] UpsertProductCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }

        [HttpPut("item/{id}", Name = "UpdateProductItem")]
        [Produces(typeof(ApiResultObject<ProductItemDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Products")]
        public async Task<ProductItemDto> UpdateProductItem([FromRoute] int id, [FromBody] UpsertProductItemCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }

        [HttpGet("find", Name = "FindProductByQrCode")]
        [Produces(typeof(ApiResultObject<ProductInfoDto>))]
        public async Task<ProductInfoDto> FindByPhone([FromQuery] GetProductByQrCode query)
        {
            return await Mediator.Send(query);
        }
    }
}