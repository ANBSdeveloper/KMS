using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Mediator;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandler
{
    public class TicketInvestmnetGetRequestListByUserHandler : QueryHandlerBase, IRequestHandler<TicketInvestmnetGetRequestListByUser, PagingResult<TicketInvestmentListItemDto>>
    {
        public TicketInvestmnetGetRequestListByUserHandler(IRequestSupplement supplement) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<TicketInvestmentListItemDto>> Handle(TicketInvestmnetGetRequestListByUser request, CancellationToken cancellationToken)
        {
            return (await Mediator.Send(new TicketInvestmnetGetListByUser()
            {
                StaffId = request.StaffId,
                MaxResult = request.MaxResult,
                Keyword = request.Keyword,
                Skip = request.Skip,
                Sort = request.Sort,
                Status = new System.Collections.Generic.List<int>() {
                    (int)TicketInvestmentStatus.RequestInvestment
                }
            }));
        }
    }
}