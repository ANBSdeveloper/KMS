using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Kms.Application.Branches.Dto;
using Cbms.Kms.Application.Branches.Query;
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
    [Route("api/v1/branches")]
    public class BranchController : AppController
    {
        public BranchController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetBranch")]
        [Produces(typeof(ApiResultObject<BranchDto>))]
        public async Task<BranchDto> Get(int id)
        {
            return await Mediator.Send(new GetBranch(id));
        }

        [HttpGet(Name = "GetBranches")]
        [Produces(typeof(ApiResultObject<PagingResult<BranchListItemDto>>))]
        public async Task<PagingResult<BranchListItemDto>> GetList([FromQuery] GetBranchList query)
        {
            return await Mediator.Send(query);
        }
    }
}