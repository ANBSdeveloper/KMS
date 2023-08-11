using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.PosmTypes.Commands;
using Cbms.Kms.Application.PosmTypes.Dto;
using Cbms.Kms.Application.PosmTypes.Query;
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
    [Route("api/v1/posm-types")]
    public class PosmTypeController : AppController
    {
        public PosmTypeController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetPosmType")]
        [Produces(typeof(ApiResultObject<PosmTypeDto>))]
        public async Task<PosmTypeDto> Get(int id)
        {
            return await Mediator.Send(new PosmTypeGet(id));
        }

        [HttpGet(Name = "GetPosmTypes")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmTypeDto>>))]
        public async Task<PagingResult<PosmTypeDto>> GetList([FromQuery] PosmTypeGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreatePosmType")]
        [Produces(typeof(ApiResultObject<PosmTypeDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmTypes")]
        public async Task<PosmTypeDto> Create([FromBody] PosmTypeUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeletePosmType")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmTypes")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new PosmTypeDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdatePosmType")]
        [Produces(typeof(ApiResultObject<PosmTypeDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmTypes")]
        public async Task<PosmTypeDto> Update([FromRoute] int id, [FromBody] PosmTypeUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}