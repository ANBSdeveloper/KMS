using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.TicketInvestments;
using Cbms.Kms.Application.TicketInvestments.Commands;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.Ticket
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/ticket-investments")]
    public class TicketInvestmentController : AppController
    {
        public TicketInvestmentController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetTicketInvestment")]
        [Produces(typeof(ApiResultObject<TicketInvestmentDto>))]
        public async Task<TicketInvestmentDto> Get(int id)
        {
            return await Mediator.Send(new TicketInvestmentGet(id));
        }

        [HttpGet("{id}/history", Name = "GetTicketInvestmentHistory")]
        [Produces(typeof(ApiResultObject<List<TicketInvestmentHistoryDto>>))]
        public async Task<List<TicketInvestmentHistoryDto>> GetHistory(int id)
        {
            return await Mediator.Send(new TicketInvestmentHistoryGet(id));
        }

        [HttpGet("{id}/tickets", Name = "GetTickets")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketDto>>))]
        public async Task<PagingResult<TicketDto>> GetTicketList(int id)
        {
            return await Mediator.Send(new TicketGetList(id));
        }

        [HttpGet("{id}/summary", Name = "GetTicketInvestmentSummary")]
        [Produces(typeof(ApiResultObject<TicketInvestmentSummaryDto>))]
        public async Task<TicketInvestmentSummaryDto> GetTicketInvestmentSummary(int id)
        {
            return await Mediator.Send(new TicketInvestmentSummaryGet(id));
        }

        [HttpGet("{id}/tracking", Name = "GetTicketInvestmentTracking")]
        [Produces(typeof(ApiResultObject<TicketInvestmentTrackingDto>))]
        public async Task<TicketInvestmentTrackingDto> GetTicketInvestmentTracking(int id)
        {
            return await Mediator.Send(new TicketInvestmentTrackingGet(id));
        }

        [HttpGet("active-program", Name = "GetActiveTicketInvestment")]
        [Produces(typeof(ApiResultObject<TicketInvestmentTrackingDto>))]
        public async Task<TicketInvestmentDto> GetActiveTicketInvestment()
        {
            return await Mediator.Send(new TicketInvestmentActiveGet());
        }

        [HttpGet("tickets/{id}", Name = "GetTicket")]
        [Produces(typeof(ApiResultObject<TicketDto>))]
        public async Task<TicketDto> GetTicket(int id)
        {
            return await Mediator.Send(new TicketGet(id));
        }

        [HttpGet("consumer-rewards/{id}", Name = "GetTicketConsumerReward")]
        [Produces(typeof(ApiResultObject<TicketConsumerRewardDto>))]
        public async Task<TicketConsumerRewardDto> GetConsumerReward(int id)
        {
            return await Mediator.Send(new TicketConsumerRewardGet(id));
        }

        [HttpPost("register", Name = "RegisterTicketInvestment")]
        [Produces(typeof(ApiResultObject<TicketInvestmentDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.Register")]
        public async Task<TicketInvestmentDto> Register([FromBody] TicketInvestmentRegisterCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpPost("update", Name = "UpdateTicketInvestment")]
        [Produces(typeof(ApiResultObject<object>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments")]
        public async Task Update([FromBody] TicketInvestmentUpdateCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("{id}/approve", Name = "ApproveTicketInvestment")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve(int id)
        {
            await Mediator.Send(new TicketInvestmentApproveCommand(id));
        }

        [HttpPost("{id}/deny", Name = "DenyTicketInvestment")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Deny(int id)
        {
            await Mediator.Send(new TicketInvestmentDenyCommand(id));
        }

        [HttpPost("{id}/progresses", Name = "CreateTicketInvestmentProgress")]
        [Produces(typeof(ApiResultObject<TicketInvestmentDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.UpdateProgress")]
        public async Task<TicketInvestmentDto> UpdateProgress(int id, [FromBody] TicketInvestmentUpsertProgressCommand command)
        {
            var entityDto = await Mediator.Send(command.WithId(id));
            return entityDto;
        }

        [HttpPut("{id}/progresses/{progressId}", Name = "UpdateTicketInvestmentProgress")]
        [Produces(typeof(ApiResultObject<TicketInvestmentDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.UpdateProgress")]
        public async Task<TicketInvestmentDto> UpdateProgress(int id, int progressId, [FromBody] TicketInvestmentUpsertProgressCommand command)
        {
            var entityDto = await Mediator.Send(command.WithId(id).WithProgressId(progressId));
            return entityDto;
        }

        [HttpPost("{id}/operate", Name = "OperateTicketInvestment")]
        [Produces(typeof(ApiResultObject<TicketInvestmentDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.Operate")]
        public async Task<TicketInvestmentDto> Operate(int id, [FromBody] TicketInvestmentOperateCommand command)
        {
            var entityDto = await Mediator.Send(command.WithId(id));
            return entityDto;
        }

        [HttpPost("{id}/accept", Name = "AcceptTicketInvestment")]
        [Produces(typeof(ApiResultObject<TicketInvestmentDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.Accept")]
        public async Task<TicketInvestmentDto> Accept(int id, [FromBody] TicketInvestmentUpsertAcceptanceCommand command)
        {
            var entityDto = await Mediator.Send(command.WithId(id));
            return entityDto;
        }

        [HttpPost("{id}/final-settlement", Name = "FinalSettlementTicketInvestment")]
        [Produces(typeof(ApiResultObject<TicketInvestmentDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.FinalSettlement")]
        public async Task<TicketInvestmentDto> FinalSettlement(int id, [FromBody] TicketInvestmentUpsertFinalSettlementCommand command)
        {
            var entityDto = await Mediator.Send(command.WithId(id));
            return entityDto;
        }

        [HttpPost("{id}/consumer-rewards/{rewardItemId}", Name = "UpdateTicketInvestmentConsumerReward")]
        [Produces(typeof(ApiResultObject<TicketConsumerRewardDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.Accept")]
        public async Task<TicketConsumerRewardDto> Accept(int id, int rewardItemId, [FromBody] TicketInvestmentUpsertConsumerRewardCommand command)
        {
            var entityDto = await Mediator.Send(command.WithId(id).WithRewardItemId(rewardItemId));
            return entityDto;
        }

        [HttpPost("{id}/sales-remark", Name = "SalesRemarkTicketInvestment")]
        [Produces(typeof(ApiResultObject<object>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.SalesRemark")]
        public async Task SalesRemark(int id, [FromBody] TicketInvestmentSalesRemarkCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPost("{id}/company-remark", Name = "CompanyRemarkTicketInvestment")]
        [Produces(typeof(ApiResultObject<object>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.CompanyRemark")]
        public async Task CompanyRemark(int id, [FromBody] TicketInvestmentCompanyRemarkCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPost("{id}/customer-development-remark", Name = "CustomerDevelopmentRemarkTicketInvestment")]
        [Produces(typeof(ApiResultObject<object>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "TicketInvestments", "TicketInvestments.CustomerDevelopmentRemark")]
        public async Task CustomerDevelopmentRemark(int id, [FromBody] TicketInvestmentCustomerDevelopmentRemarkCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpGet("byuser", Name = "GetTicketInvestmentsByUser")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketInvestmentListItemDto>>))]
        public async Task<PagingResult<TicketInvestmentListItemDto>> GetListByUser([FromQuery] TicketInvestmnetGetListByUser query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("holding", Name = "GetHoldingTicketInvestmentsByUser")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketInvestmentListItemDto>>))]
        public async Task<PagingResult<TicketInvestmentListItemDto>> GetHoldingListByUser([FromQuery] TicketInvestmnetGetHoldingListByUser query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("request", Name = "GetRequestTicketInvestmentsByUser")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketInvestmentListItemDto>>))]
        public async Task<PagingResult<TicketInvestmentListItemDto>> GetRequestListByUser([FromQuery] TicketInvestmnetGetRequestListByUser query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("approved", Name = "GetApprovedTicketInvestmentsByUser")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketInvestmentListItemDto>>))]
        public async Task<PagingResult<TicketInvestmentListItemDto>> GetApprovedListByUser([FromQuery] TicketInvestmnetGetApprovedListByUser query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("bycustomer", Name = "GetTicketInvestmentsByCustomer")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketInvestmentListItemDto>>))]
        public async Task<PagingResult<TicketInvestmentListItemDto>> GetListByCustomer([FromQuery] TicketInvestmnetGetListByCustomer query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("bytime", Name = "GetTicketInvestmentsByTime")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketInvestmentListItemDto>>))]
        public async Task<PagingResult<TicketInvestmentListItemDto>> GetListByTime([FromQuery] TicketInvestmnetGetListByTime query)
        {
            var entityDto = await Mediator.Send(query); ;
            return entityDto;
        }

        [HttpGet("getOperationPlanInvestmentList", Name = "GetOperationPlanInvestmentList")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketInvestmentListItemDto>>))]
        public async Task<PagingResult<TicketInvestmentListItemDto>> GetOperationPlanInvestmentList([FromQuery] TicketInvestmnetGetListByTime query)
        {

            var entityDto = await Mediator.Send(new TicketInvestmnetGetListByTime()
            {
                Status = { (int)TicketInvestmentStatus.Approved,
                    (int)TicketInvestmentStatus.Denied,
                    (int)TicketInvestmentStatus.Doing,
                    (int)TicketInvestmentStatus.Operated,
                    (int)TicketInvestmentStatus.Accepted,
                    (int)TicketInvestmentStatus.FinalSettlement },
                RsmStaffId = query.RsmStaffId,
                AsmStaffId = query.AsmStaffId,
                SalesSupervisorStaffId = query.SalesSupervisorStaffId,
                FromDate = query.FromDate,
                ToDate = query.ToDate,
                ByOperationDate = true,
                Keyword = query.Keyword,
                MaxResult = query.MaxResult,
                Skip = query.Skip,
                Sort = query.Sort
            });
            return entityDto;
        }

        [HttpGet("getTicketByTicketInvestmentId", Name = "GetTicketByTicketInvestmentId")]
        [Produces(typeof(ApiResultObject<PagingResult<TicketListDto>>))]
        public async Task<PagingResult<TicketListDto>> GetTicketByTicketInvestmentId([FromQuery] TicketGetListByTicketInvestmentId query)
        {
            var entityDto = await Mediator.Send(query); ;
            return entityDto;
        }



        [HttpPost("print", Name = "PrintTicket")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Tickets")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task PrintTicket([FromBody] TicketInvestmentUpdatePrintTicketQuantityCommand command)
        {
            await Mediator.Send(command);
        }

    }
}