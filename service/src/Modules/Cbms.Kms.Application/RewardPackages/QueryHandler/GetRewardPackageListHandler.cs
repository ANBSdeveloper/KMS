using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Kms.Application.RewardPackages.Query;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.RewardPackages.QueryHandler
{
    public class GetRewardPackageListHandler : QueryHandlerBase, IRequestHandler<GetRewardPackageList, PagingResult<RewardPackageListDto>>
    {
        private readonly AppDbContext _dbContext;
        public GetRewardPackageListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }
        public async Task<PagingResult<RewardPackageListDto>> Handle(GetRewardPackageList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from rewardPackages in _dbContext.RewardPackages
                        select new RewardPackageListDto()
                        {
                            Code = rewardPackages.Code,
                            Name = rewardPackages.Name,
                            IsActive = rewardPackages.IsActive,
                            Type = (int)rewardPackages.Type,
                            Id = rewardPackages.Id,
                            FromDate = rewardPackages.FromDate,
                            ToDate = rewardPackages.ToDate,
                            TotalAmount = rewardPackages.TotalAmount,
                            TotalTickets = rewardPackages.TotalTickets
                        };                       


            query = query
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive.Value)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                    x.Name.Contains(keyword));

            int totalCount = query.Count();
            query = query.SortFromString(request.Sort);
            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<RewardPackageListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}