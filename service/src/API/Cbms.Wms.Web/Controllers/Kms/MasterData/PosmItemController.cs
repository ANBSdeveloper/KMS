using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Materials.Commands;
using Cbms.Kms.Application.PosmItems.Commands;
using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Kms.Application.PosmItems.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.MasterData
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/posm-items")]
    public class PosmItemController : AppController
    {
        public PosmItemController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetPosmItem")]
        [Produces(typeof(ApiResultObject<PosmItemDto>))]
        public async Task<PosmItemDto> Get(int id)
        {
            return await Mediator.Send(new PosmItemGet(id));
        }

        [HttpGet(Name = "GetPosmItems")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmItemListDto>>))]
        public async Task<PagingResult<PosmItemListDto>> GetList([FromQuery] PosmItemGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreatePosmItem")]
        [Produces(typeof(ApiResultObject<PosmItemDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmItems")]
        public async Task<PosmItemDto> Create([FromBody] PosmItemUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeletePosmItem")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmItems")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new PosmItemDeleteCommand(id));
        }

        [HttpGet("{id}/price", Name = "GetPosmItemPrice")]
        public async Task<decimal> GetPrice(int id)
        {
            return await Mediator.Send(new PosmItemPriceGet(id));
        }

        [HttpGet("{id}/catalogs", Name = "GetPosmCatalogs")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmCatalogListDto>>))]
        public async Task<PagingResult<PosmCatalogListDto>> GetCatalogs(int id)
        {
            return await Mediator.Send(new PosmCatalogGetList() { PosmItemId = id});
        }

        [HttpGet("catalogs/{id}", Name = "GetPosmCatalog")]
        [Produces(typeof(ApiResultObject<PosmCatalogDto>))]
        public async Task<PosmCatalogDto> GetCatalog(int id)
        {
            return await Mediator.Send(new PosmCatalogGet(id));
        }

        [HttpPut("{id}", Name = "UpdatePosmItem")]
        [Produces(typeof(ApiResultObject<PosmItemDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmItems")]
        public async Task<PosmItemDto> Update([FromRoute] int id, [FromBody] PosmItemUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }

        [HttpPost("export", Name = "ExportPosmItem")]
        [Produces(typeof(Stream))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmItems")]
        public async Task<IActionResult> Export()
        {
            var fileName = await Mediator.Send(new PosmItemExportCommand());
            var stream = new FileStream(fileName, FileMode.Open);
            return File(stream, "octet/stream");
        }

        [HttpPost("import", Name = "ImportPosmItem")]
        [Produces(typeof(object))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmItems")]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            await Mediator.Send(new PosmItemImportCommand(file));
            return Ok();
        }
    }
}