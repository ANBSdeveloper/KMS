using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.PosmItems.Commands;
using Cbms.Kms.Application.PosmPrices.Commands;
using Cbms.Kms.Application.PosmPrices.Dto;
using Cbms.Kms.Application.PosmPrices.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.Posm
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/posm-prices")]
    public class PosmPriceController : AppController
    {
        public PosmPriceController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetPosmPrice")]
        [Produces(typeof(ApiResultObject<PosmPriceHeaderDto>))]
        public async Task<PosmPriceHeaderDto> Get(int id)
        {
            return await Mediator.Send(new PosmPriceHeaderGet(id));
        }

        [HttpGet(Name = "GetPosmPrices")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmPriceHeaderListDto>>))]
        public async Task<PagingResult<PosmPriceHeaderListDto>> GetList([FromQuery] PosmPriceHeaderGetList query)
        {
            var entityDto = await Mediator.Send(query);
            return entityDto;
        }

        [HttpPost(Name = "CreatePosmPrice")]
        [Produces(typeof(ApiResultObject<PosmPriceHeaderDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmPrices")]
        public async Task<PosmPriceHeaderDto> Create([FromBody] PosmPriceUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeletePosmPrice")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmPrices")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new PosmPriceHeaderDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdatePosmPrice")]
        [Produces(typeof(ApiResultObject<PosmPriceHeaderDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmPrices")]
        public async Task<PosmPriceHeaderDto> Update([FromRoute] int id, [FromBody] PosmPriceUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }

        [HttpPost("export", Name = "ExportPosmPrice")]
        [Produces(typeof(Stream))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmPrices")]
        public async Task<IActionResult> Export()
        {
            var fileName = await Mediator.Send(new PosmPriceHeaderExportCommand());
            var stream = new FileStream(fileName, FileMode.Open);
            return File(stream, "octet/stream");
        }

        [HttpPost("import", Name = "ImportPosmPrice")]
        [Produces(typeof(object))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmPrices")]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            await Mediator.Send(new PosmPriceHeaderImportCommand(file));
            return Ok();
        }
    }
}