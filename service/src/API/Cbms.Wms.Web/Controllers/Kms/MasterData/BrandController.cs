using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Brands.Commands;
using Cbms.Kms.Application.Brands.Dto;
using Cbms.Kms.Application.Brands.Query;
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
    [Route("api/v1/brands")]
    public class BrandController : AppController
    {
        public BrandController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetBrand")]
        [Produces(typeof(ApiResultObject<BrandDto>))]
        public async Task<BrandDto> Get(int id)
        {
            return await Mediator.Send(new GetBrand(id));
        }

        [HttpGet(Name = "GetBrands")]
        [Produces(typeof(ApiResultObject<PagingResult<BrandDto>>))]
        public async Task<PagingResult<BrandDto>> GetList([FromQuery] GetBrandList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateBrand")]
        [Produces(typeof(ApiResultObject<BrandDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Brands")]
        public async Task<BrandDto> Create([FromBody] UpsertBrandCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteBrand")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Brands")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new DeleteBrandCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateBrand")]
        [Produces(typeof(ApiResultObject<BrandDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Brands")]
        public async Task<BrandDto> Update([FromRoute] int id, [FromBody] UpsertBrandCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}