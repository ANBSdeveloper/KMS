using Cbms.Application.Runtime;
using Cbms.Mediator.Query.Pagination;
using Cbms.Kms.Application.Cycles.Dto;
using Cbms.Kms.Application.Cycles.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;
using Cbms.AspNetCore.Web.Models;
using Cbms.Application.Authorization;
using Cbms.Authorization;
using Cbms.Kms.Application.Cycles.Commands;

namespace Cbms.Kms.Web.Controllers.Kms.MasterData
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/cycles")]
    public class CycleController : AppController
    {
        public CycleController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetCycle")]
        [Produces(typeof(ApiResultObject<CycleDto>))]
        public async Task<CycleDto> Get(int id)
        {
            return await Mediator.Send(new GetCycle(id));
        }

        [HttpGet(Name = "GetCycles")]
        [Produces(typeof(ApiResultObject<PagingResult<CycleDto>>))]
        public async Task<PagingResult<CycleDto>> GetList([FromQuery] GetCycleList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateCycle")]
        [Produces(typeof(ApiResultObject<CycleDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Cycles")]
        public async Task<CycleDto> Create([FromBody] UpsertCycleCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteCycle")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Cycles")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new DeleteCycleCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateCycle")]
        [Produces(typeof(ApiResultObject<CycleDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Cycles")]
        public async Task<CycleDto> Update([FromRoute] int id, [FromBody] UpsertCycleCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}