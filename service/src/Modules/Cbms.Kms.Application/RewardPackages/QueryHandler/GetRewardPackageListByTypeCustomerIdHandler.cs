using Cbms.Domain.Entities;
using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Kms.Application.RewardPackages.Query;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.RewardPackages.QueryHandler
{
    public class GetRewardPackageListByTypeCustomerIdHandler : QueryHandlerBase, IRequestHandler<GetRewardPackageListByTypeCustomerId, PagingResult<RewardPackageListDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetRewardPackageListByTypeCustomerIdHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<RewardPackageListDto>> Handle(GetRewardPackageListByTypeCustomerId request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
      
            if (request.CustomerId.HasValue)
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == request.CustomerId);
                if (customer != null)
                {
                    var query = from rewardPackages in _dbContext.RewardPackages
                                 join rewardBranches in _dbContext.RewardBranches on rewardPackages.Id equals rewardBranches.RewardPackageId
                                 where rewardBranches.BranchId == customer.BranchId
                                 select  new RewardPackageListDto()
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
                        .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword))
                        .WhereIf(request.ValidDate.HasValue, x => x.FromDate <= request.ValidDate && request.ValidDate <= x.ToDate)
                        .Where(x => x.Type == (int)request.Type);

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
                throw new EntityNotFoundException(typeof(Customer), request.CustomerId);
            }
            else
            {
                var query = from rewardPackages in _dbContext.RewardPackages
                             select  new RewardPackageListDto()
                            {
                                Code = rewardPackages.Code,
                                Name = rewardPackages.Name,
                                IsActive = rewardPackages.IsActive,
                                Type = (int)rewardPackages.Type,
                                Id = rewardPackages.Id,
                                FromDate = rewardPackages.FromDate,
                                ToDate = rewardPackages.ToDate,
                                TotalTickets = rewardPackages.TotalTickets,
                                TotalAmount = rewardPackages.TotalAmount
                             };

                query = query
                    .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword))
                    .WhereIf(request.ValidDate.HasValue, x => x.FromDate <= request.ValidDate && request.ValidDate <= x.ToDate)
                    .Where(x => x.Type == (int)request.Type);

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
                    TotalCount = totalCount,
                };
            }
        }
    }
}