using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.PosmInvestments.Commands;
using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments.Query;
using Cbms.Kms.Application.PosmPrices.Commands;
using Cbms.Kms.Application.TicketInvestments.Commands;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Buffers.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/posm-investments")]
    public class PosmInvestmentController : AppController
    {
        public PosmInvestmentController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetPosmInvestment")]
        [Produces(typeof(ApiResultObject<PosmInvestmentDto>))]
        public async Task<PosmInvestmentDto> Get(int id)
        {
            return await Mediator.Send(new PosmInvestmentGet(id));
        }

        //[HttpGet("{id}/history", Name = "GetPosmInvestmentHistory")]
        //[Produces(typeof(ApiResultObject<List<PosmInvestmentHistoryDto>>))]
        //public async Task<List<PosmInvestmentHistoryDto>> GetHistory(int id)
        //{
        //    return await Mediator.Send(new PosmInvestmentHistoryGet(id));
        //}


        //[HttpGet("{id}/summary", Name = "GetPosmInvestmentSummary")]
        //[Produces(typeof(ApiResultObject<PosmInvestmentSummaryDto>))]
        //public async Task<PosmInvestmentSummaryDto> GetPosmInvestmentSummary(int id)
        //{
        //    return await Mediator.Send(new PosmInvestmentSummaryGet(id));
        //}

        //[HttpGet("{id}/tracking", Name = "GetPosmInvestmentTracking")]
        //[Produces(typeof(ApiResultObject<PosmInvestmentTrackingDto>))]
        //public async Task<PosmInvestmentTrackingDto> GetPosmInvestmentTracking(int id)
        //{
        //    return await Mediator.Send(new PosmInvestmentTrackingGet(id));
        //}

     

        //[HttpGet("tickets/{id}", Name = "GetPosm")]
        //[Produces(typeof(ApiResultObject<PosmDto>))]
        //public async Task<PosmDto> GetPosm(int id)
        //{
        //    return await Mediator.Send(new PosmGet(id));
        //}

        //[HttpGet("consumer-rewards/{id}", Name = "GetPosmConsumerReward")]
        //[Produces(typeof(ApiResultObject<PosmConsumerRewardDto>))]
        //public async Task<PosmConsumerRewardDto> GetConsumerReward(int id)
        //{
        //    return await Mediator.Send(new PosmConsumerRewardGet(id));
        //}

        [HttpPost("register", Name = "RegisterPosmInvestment")]
        [Produces(typeof(ApiResultObject<PosmInvestmentDto>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.Register")]
        public async Task<PosmInvestmentDto> Register([FromBody] PosmInvestmentRegisterCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        //[HttpPost("update", Name = "UpdatePosmInvestment")]
        //[Produces(typeof(ApiResultObject<object>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments")]
        //public async Task Update([FromBody] PosmInvestmentUpdateCommand command)
        //{
        //   await Mediator.Send(command);
        //}

        [HttpPut("{id}/asm-approve", Name = "AsmApprovePosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.AsmApprove")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve([FromRoute] int id, [FromBody] PosmInvestmentAsmApproveCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/asm-deny", Name = "AsmDenyPosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.AsmDeny")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve([FromRoute] int id, [FromBody] PosmInvestmentAsmDenyCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/rsm-approve", Name = "RsmApprovePosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.RsmApprove")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve([FromRoute] int id, [FromBody] PosmInvestmentRsmApproveCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/rsm-deny", Name = "RsmDenyPosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.RsmDeny")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve([FromRoute] int id, [FromBody] PosmInvestmentRsmDenyCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/trade-approve", Name = "TradeApprovePosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.TradeApprove")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve([FromRoute] int id, [FromBody] PosmInvestmentTradeApproveCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("trade-multi-approve", Name = "TradeMultiApprovePosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.TradeApprove")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task MultiApprove([FromBody] PosmInvestmentTradeMultiApproveCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPut("{id}/trade-deny", Name = "TradeDenyPosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.TradeDeny")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Deny([FromRoute] int id, [FromBody] PosmInvestmentTradeDenyCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("trade-multi-deny", Name = "TradeMultiDenyPosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.TradeDeny")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task MultiDeny([FromBody] PosmInvestmentTradeMultiDenyCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPut("{id}/director-approve", Name = "DirectorApprovePosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.DirectorApprove")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve([FromRoute] int id, [FromBody] PosmInvestmentDirectorApproveCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("director-multi-approve", Name = "DirectorMultiApprovePosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.DirectorApprove")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task MultiApprove([FromBody] PosmInvestmentDirectorMultiApproveCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPut("{id}/director-deny", Name = "DirectorDenyPosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.DirectorDeny")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Deny([FromRoute] int id, [FromBody] PosmInvestmentDirectorDenyCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("director-multi-deny", Name = "DirectorMultiDenyPosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.DirectorDeny")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task MultiDeny([FromBody] PosmInvestmentDirectorMultiDenyCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPut("{id}/supply-confirm-request", Name = "SupplyApprovePosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.SupplyConfirmRequest")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve([FromRoute] int id, [FromBody] PosmInvestmentSupplyConfirmRequestCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/supply-deny-request", Name = "SupplyDenyPosmInvestment")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.SupplyDenyRequest")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Approve([FromRoute] int id, [FromBody] PosmInvestmentSupplyDenyRequestCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/suggest-budget", Name = "SuggestBudget")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.SuggestBudget")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task SuggestBudget([FromRoute] int id, [FromBody] PosmInvestmentSupSuggestCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/asm-confirm-suggest", Name = "AsmConfirmSuggest")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.AsmConfirmSuggest")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ConfirmSuggest([FromRoute] int id, [FromBody] PosmInvestmentAsmConfirmSuggestCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/rsm-confirm-suggest", Name = "RsmConfirmSuggest")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.RsmConfirmSuggest")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ConfirmSuggest([FromRoute] int id, [FromBody] PosmInvestmentRsmConfirmSuggestCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/trade-confirm-suggest", Name = "TradeConfirmSuggest")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.TradeConfirmSuggest")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ConfirmSuggest([FromRoute] int id, [FromBody] PosmInvestmentTradeConfirmSuggestCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/marketing-confirm-produce", Name = "MarketingConfirmProduce")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.MarketingConfirmProduce")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ConfirmProduce([FromRoute] int id, [FromBody] PosmInvestmentMarketingConfirmProduceCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/sup-confirm-produce", Name = "SupConfirmProduce")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.SupConfirmProduce")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ConfirmProduce([FromRoute] int id, [FromBody] PosmInvestmentSupConfirmProduceCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/supply-confirm-produce", Name = "SupplyConfirmProduce")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.SupplyConfirmProduce")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ConfirmProduce([FromRoute] int id, [FromBody] PosmInvestmentSupplyConfirmProduceCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/accept", Name = "Accept")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.Accept")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task Accept([FromRoute] int id, [FromBody] PosmInvestmentSupAcceptCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/confirm-accept-1", Name = "ConfirmAccept1")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.ConfirmAccept1")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task CnnfirmAccept([FromRoute] int id, [FromBody] PosmInvestmentAsmConfirmAcceptCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("{id}/confirm-accept-2", Name = "ConfirmAccept2")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.ConfirmAccept2")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task CnnfirmAccept([FromRoute] int id, [FromBody] PosmInvestmentTradeConfirmAcceptCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPut("multi-confirm-accept-2", Name = "MultiConfirmAccept2")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.ConfirmAccept2")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task MultiConfirmAccept([FromBody] PosmInvestmentTradeMultiConfirmAcceptCommand command)
        {
            await Mediator.Send(command);
        }

        //[HttpPost("{id}/deny", Name = "DenyPosmInvestment")]
        //[Produces(typeof(ApiResultObject<object>))]
        //public async Task Deny(int id)
        //{
        //    await Mediator.Send(new PosmInvestmentDenyCommand(id));
        //}

        //[HttpPost("{id}/progresses", Name = "CreatePosmInvestmentProgress")]
        //[Produces(typeof(ApiResultObject<PosmInvestmentDto>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.UpdateProgress")]
        //public async Task<PosmInvestmentDto> UpdateProgress(int id, [FromBody] PosmInvestmentUpsertProgressCommand command)
        //{
        //    var entityDto = await Mediator.Send(command.WithId(id));
        //    return entityDto;
        //}

        //[HttpPut("{id}/progresses/{progressId}", Name = "UpdatePosmInvestmentProgress")]
        //[Produces(typeof(ApiResultObject<PosmInvestmentDto>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.UpdateProgress")]
        //public async Task<PosmInvestmentDto> UpdateProgress(int id, int progressId, [FromBody] PosmInvestmentUpsertProgressCommand command)
        //{
        //    var entityDto = await Mediator.Send(command.WithId(id).WithProgressId(progressId));
        //    return entityDto;
        //}

        //[HttpPost("{id}/operate", Name = "OperatePosmInvestment")]
        //[Produces(typeof(ApiResultObject<PosmInvestmentDto>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.Operate")]
        //public async Task<PosmInvestmentDto> Operate(int id, [FromBody] PosmInvestmentOperateCommand command)
        //{
        //    var entityDto = await Mediator.Send(command.WithId(id));
        //    return entityDto;
        //}

        //[HttpPost("{id}/accept", Name = "AcceptPosmInvestment")]
        //[Produces(typeof(ApiResultObject<PosmInvestmentDto>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.Accept")]
        //public async Task<PosmInvestmentDto> Accept(int id, [FromBody] PosmInvestmentUpsertAcceptanceCommand command)
        //{
        //    var entityDto = await Mediator.Send(command.WithId(id));
        //    return entityDto;
        //}

        //[HttpPost("{id}/final-settlement", Name = "FinalSettlementPosmInvestment")]
        //[Produces(typeof(ApiResultObject<PosmInvestmentDto>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.FinalSettlement")]
        //public async Task<PosmInvestmentDto> FinalSettlement(int id, [FromBody] PosmInvestmentUpsertFinalSettlementCommand command)
        //{
        //    var entityDto = await Mediator.Send(command.WithId(id));
        //    return entityDto;
        //}

        //[HttpPost("{id}/consumer-rewards/{rewardItemId}", Name = "UpdatePosmInvestmentConsumerReward")]
        //[Produces(typeof(ApiResultObject<PosmConsumerRewardDto>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.Accept")]
        //public async Task<PosmConsumerRewardDto> Accept(int id, int rewardItemId, [FromBody] PosmInvestmentUpsertConsumerRewardCommand command)
        //{
        //    var entityDto = await Mediator.Send(command.WithId(id).WithRewardItemId(rewardItemId));
        //    return entityDto;
        //}

        //[HttpPost("{id}/sales-remark", Name = "SalesRemarkPosmInvestment")]
        //[Produces(typeof(ApiResultObject<object>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.SalesRemark")]
        //public async Task SalesRemark(int id, [FromBody] PosmInvestmentSalesRemarkCommand command)
        //{
        //    await Mediator.Send(command.WithId(id));
        //}

        //[HttpPost("{id}/company-remark", Name = "CompanyRemarkPosmInvestment")]
        //[Produces(typeof(ApiResultObject<object>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.CompanyRemark")]
        //public async Task CompanyRemark(int id, [FromBody] PosmInvestmentCompanyRemarkCommand command)
        //{
        //    await Mediator.Send(command.WithId(id));
        //}

        //[HttpPost("{id}/customer-development-remark", Name = "CustomerDevelopmentRemarkPosmInvestment")]
        //[Produces(typeof(ApiResultObject<object>))]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.CustomerDevelopmentRemark")]
        //public async Task CustomerDevelopmentRemark(int id, [FromBody] PosmInvestmentCustomerDevelopmentRemarkCommand command)
        //{
        //    await Mediator.Send(command.WithId(id));
        //}

        [HttpGet("byuser", Name = "GetPosmInvestmentsByUser")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmInvestmentListDto>>))]
        public async Task<PagingResult<PosmInvestmentListDto>> GetListByUser([FromQuery] PosmInvestmentGetListByUser query)
        {
            return await Mediator.Send(query);
        }

        //[HttpGet("holding", Name = "GetHoldingPosmInvestmentsByUser")]
        //[Produces(typeof(ApiResultObject<PagingResult<PosmInvestmentListItemDto>>))]
        //public async Task<PagingResult<PosmInvestmentListItemDto>> GetHoldingListByUser([FromQuery] PosmInvestmnetGetHoldingListByUser query)
        //{
        //    return await Mediator.Send(query);
        //}

        //[HttpGet("request", Name = "GetRequestPosmInvestmentsByUser")]
        //[Produces(typeof(ApiResultObject<PagingResult<PosmInvestmentListItemDto>>))]
        //public async Task<PagingResult<PosmInvestmentListItemDto>> GetRequestListByUser([FromQuery] PosmInvestmnetGetRequestListByUser query)
        //{
        //    return await Mediator.Send(query);
        //}

        //[HttpGet("approved", Name = "GetApprovedPosmInvestmentsByUser")]
        //[Produces(typeof(ApiResultObject<PagingResult<PosmInvestmentListItemDto>>))]
        //public async Task<PagingResult<PosmInvestmentListItemDto>> GetApprovedListByUser([FromQuery] PosmInvestmnetGetApprovedListByUser query)
        //{
        //    return await Mediator.Send(query);

        [HttpGet("items/{id}", Name = "GetPosmInvestmentItem")]
        [Produces(typeof(ApiResultObject<PosmInvestmentItemDto>))]
        public async Task<PosmInvestmentItemDto> GetPosmInvestmentItem(int id)
        {
            return await Mediator.Send(new PosmInvestmentItemGet(id));
        }

        [HttpGet("items/{id}/operation", Name = "GetPosmInvestmentItemOperation")]
        [Produces(typeof(ApiResultObject<PosmInvestmentItemOperationDto>))]
        public async Task<PosmInvestmentItemOperationDto> GetPosmInvestmentItemOperation(int id)
        {
            return await Mediator.Send(new PosmInvestmentItemOperationGet(id));
        }

        [HttpGet("items/{id}/history", Name = "GetPosmInvestmentItemHistory")]
        [Produces(typeof(ApiResultObject<PosmInvestmentItemHistoryDto>))]
        public async Task<PosmInvestmentItemHistoryDto> GetItemListByCustomer(int id)
        {
            return await Mediator.Send(new PosmInvestmentItemHistoryGet(id));
        }

        [AllowAnonymous]
        [HttpGet("items/{id}/operation-images/{index}", Name = "GetPosmInvestmentItemOperationImage")]
        [Produces(typeof(ApiResultObject<PosmInvestmentItemHistoryDto>))]
        public async Task<IActionResult> GetItemOperationImage(int id, int index)
        {
            var imageData = await Mediator.Send(new PosmInvestmentItemOperationImageGet(id, index));
            try
            {

                var stream = new MemoryStream(Convert.FromBase64String(imageData));
                return new FileStreamResult(stream, "image/png");
            }
            catch
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("items/{id}/survey-images/{index}", Name = "GetPosmInvestmentItemSurveyImage")]
        [Produces(typeof(ApiResultObject<PosmInvestmentItemHistoryDto>))]
        public async Task<IActionResult> GetItemSurveyImage(int id, int index)
        {
            var imageData = await Mediator.Send(new PosmInvestmentItemSurveyImageGet(id, index));
            try
            {

                var stream = new MemoryStream(Convert.FromBase64String(imageData));
                return new FileStreamResult(stream, "image/png");
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("items/byuser", Name = "GetPosmInvestmentItemsByUser")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmInvestmentItemExtDto>>))]
        public async Task<PagingResult<PosmInvestmentItemExtDto>> GetItemListByCustomer([FromQuery] PosmInvestmentItemGetListByUser query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("items/bycustomer", Name = "GetPosmInvestmentItemsByCustomer")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmInvestmentItemExtDto>>))]
        public async Task<PagingResult<PosmInvestmentItemExtDto>> GetItemListByCustomer([FromQuery] PosmInvestmentItemGetListByCustomer query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("bytime", Name = "GetPosmInvestmentsByTime")]
        [Produces(typeof(ApiResultObject<PagingResult<PosmInvestmentListDto>>))]
        public async Task<PagingResult<PosmInvestmentListDto>> GetListByTime([FromQuery] PosmInvestmentGetListByTime query)
        {
            var entityDto = await Mediator.Send(query); ;
            return entityDto;
        }

        [HttpPost("{id}/sales-remark", Name = "SalesRemarkPosmInvestment")]
        [Produces(typeof(ApiResultObject<object>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.SalesRemark")]
        public async Task SalesRemark(int id, [FromBody] PosmInvestmentSalesRemarkCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }

        [HttpPost("{id}/company-remark", Name = "CompanyRemarkPosmInvestment")]
        [Produces(typeof(ApiResultObject<object>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.CompanyRemark")]
        public async Task CompanyRemark(int id, [FromBody] PosmInvestmentCompanyRemarkCommand command)
        {
            await Mediator.Send(command.WithId(id));
        }


        [HttpPost("import", Name = "ImportPosmInvestment")]
        [Produces(typeof(object))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "PosmInvestments", "PosmInvestments.ImportHistory")]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            await Mediator.Send(new PosmInvestmentImportHistoryCommand(file));
            return Ok();
        }

        //[HttpGet("getOperationPlanInvestmentList", Name = "GetOperationPlanInvestmentList")]
        //[Produces(typeof(ApiResultObject<PagingResult<PosmInvestmentListItemDto>>))]
        //public async Task<PagingResult<PosmInvestmentListItemDto>> GetOperationPlanInvestmentList([FromQuery] PosmInvestmnetGetListByTime query)
        //{

        //    var entityDto = await Mediator.Send(new PosmInvestmnetGetListByTime()
        //    {
        //        Status = { (int)PosmInvestmentStatus.Approved, 
        //            (int)PosmInvestmentStatus.Denied, 
        //            (int)PosmInvestmentStatus.Doing, 
        //            (int)PosmInvestmentStatus.Operated, 
        //            (int)PosmInvestmentStatus.Accepted, 
        //            (int)PosmInvestmentStatus.FinalSettlement },
        //        RsmStaffId = query.RsmStaffId,
        //        AsmStaffId = query.AsmStaffId,
        //        SalesSupervisorStaffId = query.SalesSupervisorStaffId,
        //        FromDate = query.FromDate,
        //        ToDate = query.ToDate,
        //        ByOperationDate = true,
        //        Keyword = query.Keyword,
        //        MaxResult = query.MaxResult,
        //        Skip = query.Skip,
        //        Sort = query.Sort
        //    });
        //    return entityDto;
        //}

        //[HttpGet("getPosmByPosmInvestmentId", Name = "GetPosmByPosmInvestmentId")]
        //[Produces(typeof(ApiResultObject<PagingResult<PosmListDto>>))]
        //public async Task<PagingResult<PosmListDto>> GetPosmByPosmInvestmentId([FromQuery] PosmGetListByPosmInvestmentId query)
        //{
        //    var entityDto = await Mediator.Send(query); ;
        //    return entityDto;
        //}



        //[HttpPost("print", Name = "PrintPosm")]
        //[ClaimRequirementAny(CbmsClaimTypes.Permission, "Posms")]
        //[Produces(typeof(ApiResultObject<object>))]
        //public async Task PrintPosm([FromBody] PosmInvestmentUpdatePrintPosmQuantityCommand command)
        //{
        //    await Mediator.Send(command);
        //}

    }
    }