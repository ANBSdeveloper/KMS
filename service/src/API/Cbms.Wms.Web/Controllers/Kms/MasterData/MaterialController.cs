using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Materials.Commands;
using Cbms.Kms.Application.Materials.Dto;
using Cbms.Kms.Application.Materials.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.MasterData
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/materials")]
    public class MaterialController : AppController
    {
        public MaterialController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetMaterial")]
        [Produces(typeof(ApiResultObject<MaterialDto>))]
        public async Task<MaterialDto> Get(int id)
        {
            return await Mediator.Send(new MaterialGet(id));
        }

        [HttpGet(Name = "GetMaterials")]
        [Produces(typeof(ApiResultObject<PagingResult<MaterialListItemDto>>))]
        public async Task<PagingResult<MaterialListItemDto>> GetList([FromQuery] MaterialGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateMaterial")]
        [Produces(typeof(ApiResultObject<MaterialDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Materials")]
        public async Task<MaterialDto> Create([FromBody] MaterialUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteMaterial")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Materials")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new MaterialDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateMaterial")]
        [Produces(typeof(ApiResultObject<MaterialDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Materials")]
        public async Task<MaterialDto> Update([FromRoute] int id, [FromBody] MaterialUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
        [HttpPost("export", Name = "ExportMaterial")]
        [Produces(typeof(Stream))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Materials")]
        public async Task<IActionResult> Export()
        {
            var fileName = await Mediator.Send(new MaterialExportCommand());
            var stream = new FileStream(fileName, FileMode.Open);
            return File(stream, "octet/stream");
        }

        [HttpPost("import", Name = "ImportMaterial")]
        [Produces(typeof(object))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Materials")]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            await Mediator.Send(new MaterialImportCommand(file));
            return Ok();
        }
    }
}
