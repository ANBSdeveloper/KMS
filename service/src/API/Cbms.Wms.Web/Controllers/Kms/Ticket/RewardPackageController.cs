using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.RewardPackages.Commands;
using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Kms.Application.RewardPackages.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.Ticket
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/rewardPackages")]
    public class RewardPackageController : AppController
    {
        public RewardPackageController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetRewardPackage")]
        [Produces(typeof(ApiResultObject<RewardPackageDto>))]
        public async Task<RewardPackageDto> Get(int id)
        {
            return await Mediator.Send(new GetRewardPackage(id));
        }

        [HttpGet(Name = "GetRewardPackages")]
        [Produces(typeof(ApiResultObject<PagingResult<RewardPackageListDto>>))]
        public async Task<PagingResult<RewardPackageListDto>> GetList([FromQuery] GetRewardPackageList query)
        {
            var entityDto = await Mediator.Send(query);
            return entityDto;
        }

        [HttpGet("getRewardPackageListByTypeCustomer", Name = "GetRewardPackageListByTypeCustomerId")]
        [Produces(typeof(ApiResultObject<PagingResult<RewardPackageListDto>>))]
        public async Task<PagingResult<RewardPackageListDto>> GetListRewardPackage([FromQuery] GetRewardPackageListByTypeCustomerId query)
        {
            var entityDto = await Mediator.Send(query);
            return entityDto;
        }

        [HttpPost(Name = "CreateRewardPackage")]
        [Produces(typeof(ApiResultObject<RewardPackageDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "RewardPackages")]
        public async Task<RewardPackageDto> Create([FromBody] RewardPackageUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteRewardPackage")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "RewardPackages")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new RewardPackageDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateRewardPackage")]
        [Produces(typeof(ApiResultObject<RewardPackageDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "RewardPackages")]
        public async Task<RewardPackageDto> Update([FromRoute] int id, [FromBody] RewardPackageUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }

    }
}