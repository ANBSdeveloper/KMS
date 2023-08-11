using Cbms.Kms.Application.Investments.Dto;
using Cbms.Kms.Application.Investments.Query;
using Cbms.Kms.Application.PosmInvestments.Query;
using Cbms.Kms.Application.PosmInvestments.QueryHandler;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Investments.QueryHandlers
{
    public class InvestmentGetHandler : QueryHandlerBase, IRequestHandler<InvestmentGet, InvestmentDto>
    {
        private readonly AppDbContext _dbContext;

        public InvestmentGetHandler(
            IRequestSupplement supplement,
            AppDbContext dbContext
        ) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _dbContext = dbContext;
        }

        public async Task<InvestmentDto> Handle(InvestmentGet request, CancellationToken cancellationToken)
        {
            int requestQuantity = (await Mediator.Send(new TicketInvestmnetGetListByUser()
            {
                Status = new System.Collections.Generic.List<int>() { (int)TicketInvestmentStatus.RequestInvestment }
            })).TotalCount;

            int holdingQuantity = (await Mediator.Send(new TicketInvestmnetGetListByUser()
            {
                Status = new System.Collections.Generic.List<int>() {
                    (int)TicketInvestmentStatus.ConfirmedRequestInvestment,
                    (int)TicketInvestmentStatus.ValidRequestInvestment1,
                    (int)TicketInvestmentStatus.ValidRequestInvestment2,
                    (int)TicketInvestmentStatus.ConfirmedInvestment,
                    (int)TicketInvestmentStatus.ApproveInvestment
                }
            })).TotalCount;

            int approvedQuantity = (await Mediator.Send(new TicketInvestmnetGetListByUser()
            {
                Status = new System.Collections.Generic.List<int>() { 
                    (int)TicketInvestmentStatus.Approved, 
                    (int)TicketInvestmentStatus.Doing, 
                    (int)TicketInvestmentStatus.Accepted, 
                    (int)TicketInvestmentStatus.Operated, 
                    (int)TicketInvestmentStatus.FinalSettlement }
            })).TotalCount;

            int requestPosmQuantity = (await Mediator.Send(new PosmInvestmentItemGetListByUser()
            {
                Status = new System.Collections.Generic.List<int>() {
                    (int)PosmInvestmentItemStatus.Request,
                }
            })).TotalCount;
            int holdingPosmQuantity = (await Mediator.Send(new PosmInvestmentItemGetListByUser()
            {
                Status = new System.Collections.Generic.List<int>() {
                    (int)PosmInvestmentItemStatus.AsmApprovedRequest,
                    (int)PosmInvestmentItemStatus.RsmApprovedRequest,
                    (int)PosmInvestmentItemStatus.TradeApprovedRequest,
                }
            })).TotalCount;

            int approvedPosmQuantity = (await Mediator.Send(new PosmInvestmentItemGetListByUser()
            {
                Status = new System.Collections.Generic.List<int>() {
                    (int)PosmInvestmentItemStatus.DirectorApprovedRequest,
                    (int)PosmInvestmentItemStatus.ValidOrder,
                    (int)PosmInvestmentItemStatus.InvalidOrder,
                    (int)PosmInvestmentItemStatus.SupSuggestedUpdateCost,
                    (int)PosmInvestmentItemStatus.AsmConfirmedUpdateCost,
                    (int)PosmInvestmentItemStatus.RsmConfirmedUpdateCost,
                    (int)PosmInvestmentItemStatus.TradeApprovedRequest,
                    (int)PosmInvestmentItemStatus.ConfirmedAccept1,
                    (int)PosmInvestmentItemStatus.ConfirmedAccept2,
                    (int)PosmInvestmentItemStatus.ConfirmedProduce1,
                    (int)PosmInvestmentItemStatus.ConfirmedProduce2,
                    (int)PosmInvestmentItemStatus.ConfirmedVendorProduce,
                    (int)PosmInvestmentItemStatus.Accepted
                }
            })).TotalCount;
            var investmentDto = new InvestmentDto()
            {
                Ticket = new InvestmentDto.InvestmentAccumulateDto()
                {
                    HoldingQuantity = holdingQuantity,
                    RequestQuantity = requestQuantity,
                    ApprovedQuantity = approvedQuantity
                },
                GoldHour = new InvestmentDto.InvestmentAccumulateDto(),
                Pg = new InvestmentDto.InvestmentAccumulateDto(),
                Posm = new InvestmentDto.InvestmentAccumulateDto()
                {
                    HoldingQuantity = holdingPosmQuantity,
                    RequestQuantity = requestPosmQuantity,
                    ApprovedQuantity = approvedPosmQuantity

                }
            };

            return investmentDto;
        }
    }
}