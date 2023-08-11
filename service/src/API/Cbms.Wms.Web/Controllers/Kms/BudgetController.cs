using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Budgets.Commands;
using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Application.Budgets.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/budgets")]
    public class BudgetController : AppController
    {
        public BudgetController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetBudget")]
        [Produces(typeof(ApiResultObject<BudgetDto>))]
        public async Task<BudgetDto> Get(int id)
        {
            return await Mediator.Send(new BudgetGet(id));
        }

        [HttpGet(Name = "GetBudgets")]
        [Produces(typeof(ApiResultObject<PagingResult<BudgetListItemDto>>))]
        public async Task<PagingResult<BudgetListItemDto>> GetList([FromQuery] BudgetGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("initDetail", Name = "GetBudgetInitDetail")]
        [Produces(typeof(ApiResultObject<BudgetInitDetailDto>))]
        public async Task<BudgetInitDetailDto> GetStaffList([FromQuery] BudgetGetInitDetail query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateBudget")]
        [Produces(typeof(ApiResultObject<BudgetDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Budgets")]
        public async Task<BudgetDto> Create([FromBody] BudgetUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteBudget")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Budgets")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new BudgetDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateBudget")]
        [Produces(typeof(ApiResultObject<BudgetDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Budgets", "Budgets.AllocateArea", "Budgets.AllocateBranch")]
        public async Task<BudgetDto> Update([FromRoute] int id, [FromBody] BudgetUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }

        //[HttpGet("getBudgetDetailById", Name = "GetBudgetDetailById")]
        //[Produces(typeof(ApiResultObject<BudgetDetailGetByIdDto>))]
        //public async Task<BudgetDetailGetByIdDto> GetBudgetDetailById(int cycleId, int staffId, int investmentType)
        //{
        //    return await Mediator.Send(new BudgetDetailGetById(cycleId, staffId, investmentType));
        //}

        [HttpGet("history", Name = "GetBudgetHistoryByUser")]
        [Produces(typeof(ApiResultObject<List<BudgetHistoryByUserDto>>))]
        public async Task<List<BudgetHistoryByUserDto>> GetBudgetHistoryByUser([FromQuery] GetBudgetHistoryByUser query)
        {
            return await Mediator.Send(query);
        }
    }
}