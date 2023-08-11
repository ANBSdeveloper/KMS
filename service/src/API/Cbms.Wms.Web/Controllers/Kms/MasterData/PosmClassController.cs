using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.PosmClasses.Commands;
using Cbms.Kms.Application.PosmClasses.Dto;
using Cbms.Kms.Application.PosmClasses.Query;
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
    [Route("api/v1/posm-classes")]
    public class PosmClassController : AppController
    {
        public PosmClassController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetPosmClass")]
        [Produces(typeof(ApiResultObject<PosmClassDto>))]
        public async Task<PosmClassDto> Get(int id)
        {
            return await Mediator.Send(new PosmClassGet(id));
        }

        [HttpGet(Name = "GetPosmClasses")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmClassDto>>))]
        public async Task<PagingResult<PosmClassDto>> GetList([FromQuery] PosmClassGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreatePosmClass")]
        [Produces(typeof(ApiResultObject<PosmClassDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmClasses")]
        public async Task<PosmClassDto> Create([FromBody] PosmClassUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeletePosmClass")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmClasses")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new PosmClassDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdatePosmClass")]
        [Produces(typeof(ApiResultObject<PosmClassDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "PosmClasses")]
        public async Task<PosmClassDto> Update([FromRoute] int id, [FromBody] PosmClassUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}