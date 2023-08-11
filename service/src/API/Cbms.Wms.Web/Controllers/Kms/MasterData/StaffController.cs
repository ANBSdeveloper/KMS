using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.Application.Staffs.Commands;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Staffs.Commands;
using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Kms.Application.Staffs.Query;
using Cbms.Kms.Domain;
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
    [Route("api/v1/staffs")]
    public class StaffController : AppController
    {
        public StaffController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("info", Name = "GetStaffInfo")]
        [Produces(typeof(ApiResultObject<StaffDto>))]
        public async Task<StaffDto> GetInfo()
        {
            return await Mediator.Send(new StaffGetByUser());
        }

        [HttpGet("{id}", Name = "GetStaff")]
        [Produces(typeof(ApiResultObject<StaffDto>))]
        public async Task<StaffDto> Get([FromRoute] int id)
        {
            return await Mediator.Send(new StaffGet(id));
        }

        [HttpGet("sales-supervisor", Name = "GetSalesSuppervisorStaffs")]
        [Produces(typeof(ApiResultObject<PagingResult<StaffListDto>>))]
        public async Task<PagingResult<StaffListDto>> GetSalesSupervisorList([FromQuery] StaffGetSalesSupervisorList query)
        {
            var entityDto = await Mediator.Send(new StaffGetListByRole()
            {
                SupervisorId = query.SupervisorId,
                StaffTypeCode = KmsConsts.SalesSupervisorRole,
                Keyword = query.Keyword,
                MaxResult = query.MaxResult,
                Skip = query.Skip,
                Sort = query.Sort
            });
            return entityDto;
        }

        [HttpGet("get-staff-by-roleId", Name = "GetStaffByRoleId")]
        [Produces(typeof(ApiResultObject<StaffListDto>))]
        public async Task<StaffListDto> GetStaffByRoleId([FromQuery] StaffGetByRole query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("asm", Name = "GetAsmStaffs")]
        [Produces(typeof(ApiResultObject<PagingResult<StaffListDto>>))]
        public async Task<PagingResult<StaffListDto>> GetAsmListByRoles([FromQuery] StaffGetAsmList query)
        {
            var entityDto = await Mediator.Send(new StaffGetListByRole()
            {
                SupervisorId = query.SupervisorId,
                StaffTypeCode = KmsConsts.AsmRole,
                Keyword = query.Keyword,
                MaxResult = query.MaxResult,
                Skip = query.Skip,
                Sort = query.Sort
            });
            return entityDto;
        }

        [HttpGet("rsm", Name = "GetRsmStaffs")]
        [Produces(typeof(ApiResultObject<PagingResult<StaffListDto>>))]
        public async Task<PagingResult<StaffListDto>> GetRsmList([FromQuery] StaffGetRsmList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet(Name = "GetStaffs")]
        [Produces(typeof(ApiResultObject<PagingResult<StaffListDto>>))]
        public async Task<PagingResult<StaffListDto>> GetList([FromQuery] StaffGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}/update-credit-point", Name = "UpdateCreditPoint")]
        [Produces(typeof(ApiResultObject<object>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Staffs", "Staffs.UpdateSsCreditPoint", "Staffs.UpdateAsmCreditPoint", "Staffs.UpdateRsmCreditPoint")]
        public async Task SalesRemark(int id, [FromBody] StaffUpdateCreditPointCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        // Fake register
        [HttpPost("registerNew", Name = "RegisterStaff")]
        [Produces(typeof(ApiResultObject<object>))]
        [AllowAnonymous]
        public async Task RegisterNew([FromBody] StaffRegisterCommand command)
        {
            await Mediator.Send(command);
        }
    }
}