using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.SubProductClasses.Commands;
using Cbms.Kms.Application.SubProductClasses.Dto;
using Cbms.Kms.Application.SubProductClasses.Query;
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
    [Route("api/v1/sub-product-classes")]
    public class SubProductClassController : AppController
    {
        public SubProductClassController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetSubProductClass")]
        [Produces(typeof(ApiResultObject<SubProductClassDto>))]
        public async Task<SubProductClassDto> Get(int id)
        {
            return await Mediator.Send(new SubProductClassGet(id));
        }

        [HttpGet(Name = "GetSubProductClasses")]
        [Produces(typeof(ApiResultObject<PagingResult<SubProductClassDto>>))]
        public async Task<PagingResult<SubProductClassDto>> GetList([FromQuery] SubProductClassGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateSubProductClass")]
        [Produces(typeof(ApiResultObject<SubProductClassDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "SubProductClasses")]
        public async Task<SubProductClassDto> Create([FromBody] SubProductClassUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteSubProductClass")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "SubProductClasses")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new SubProductClassDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateSubProductClass")]
        [Produces(typeof(ApiResultObject<SubProductClassDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "SubProductClasses")]
        public async Task<SubProductClassDto> Update([FromRoute] int id, [FromBody] SubProductClassUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}