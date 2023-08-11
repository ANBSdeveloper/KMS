using Cbms.Application.Runtime;
using Cbms.Mediator.Query.Pagination;
using Cbms.Kms.Application.ProductPoints.Dto;
using Cbms.Kms.Application.ProductPoints.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;
using Cbms.AspNetCore.Web.Models;
using Cbms.Application.Authorization;
using Cbms.Authorization;
using Cbms.Kms.Application.ProductPoints.Commands;
using System.IO;
using Cbms.Kms.Application.Materials.Commands;
using Microsoft.AspNetCore.Http;

namespace Cbms.Kms.Web.Controllers.Kms.Ticket
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/product-points")]
    public class ProductPointController : AppController
    {
        public ProductPointController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetProductPoint")]
        [Produces(typeof(ApiResultObject<ProductPointDto>))]
        public async Task<ProductPointDto> Get(int id)
        {
            return await Mediator.Send(new ProductPointGet(id));
        }

        [HttpGet(Name = "GetProductPoints")]
        [Produces(typeof(ApiResultObject<PagingResult<ProductPointListItemDto>>))]
        public async Task<PagingResult<ProductPointListItemDto>> GetList([FromQuery] ProductPointGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateProductPoint")]
        [Produces(typeof(ApiResultObject<ProductPointDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "ProductPoints")]
        public async Task<ProductPointDto> Create([FromBody] ProductPointUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }
        [HttpPut("{id}", Name = "UpdateProductPoint")]
        [Produces(typeof(ApiResultObject<ProductPointDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "ProductPoints")]
        public async Task<ProductPointDto> Update([FromRoute] int id, [FromBody] ProductPointUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.WithId(id));
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteProductPoint")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "ProductPoints")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new ProductPointDeleteCommand(id));
        }

        [HttpPost("export", Name = "ExportProductPoint")]
        [Produces(typeof(Stream))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Materials")]
        public async Task<IActionResult> Export()
        {
            var fileName = await Mediator.Send(new ProductPointExportCommand());
            var stream = new FileStream(fileName, FileMode.Open);
            return File(stream, "octet/stream");
        }

        [HttpPost("import", Name = "ImportProductPoint")]
        [Produces(typeof(object))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Materials")]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            await Mediator.Send(new ProductPointImportCommand(file));
            return Ok();
        }

    }
}