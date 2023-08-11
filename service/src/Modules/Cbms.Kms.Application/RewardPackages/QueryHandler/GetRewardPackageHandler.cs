using Cbms.Domain.Repositories;
using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Kms.Application.RewardPackages.Query;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.RewardPackages.QueryHandler
{
    public class GetRewardPackageHandler : QueryHandlerBase, IRequestHandler<GetRewardPackage, RewardPackageDto>
    {
        private readonly IRepository<RewardPackage, int> _repository;
        private readonly AppDbContext _dbContext;

        public GetRewardPackageHandler(IRequestSupplement supplement, IRepository<RewardPackage, int> repository, AppDbContext dbContext) : base(supplement)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<RewardPackageDto> Handle(GetRewardPackage request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAsync(request.Id);

            var rewardItemDtos = await (from rewardItem in _dbContext.RewardItems
                                        join product in _dbContext.Products on rewardItem.ProductId equals product.Id into productLeft
                                        from product in productLeft.DefaultIfEmpty()
                                        join productUnit in _dbContext.ProductUnits on rewardItem.ProductUnitId equals productUnit.Id into productUnitLeft
                                        from productUnit in productUnitLeft.DefaultIfEmpty()

                                        where rewardItem.RewardPackageId == entity.Id
                                        select new RewardItemDto()
                                        {
                                            Id = rewardItem.Id,
                                            Code = rewardItem.Code,
                                            Name = rewardItem.Name,
                                            DocumentLink = rewardItem.DocumentLink,
                                            Price = rewardItem.Price,
                                            Quantity = rewardItem.Quantity,
                                            ProductId = rewardItem.ProductId,
                                            ProductCode = product.Code,
                                            ProductName = product.Name,
                                            ProductUnitId = rewardItem.ProductUnitId,
                                            ProductUnitCode = productUnit.Code,
                                            ProductUnitName = productUnit.Name,
                                            CreationTime = rewardItem.CreationTime,
                                            CreatorUserId =rewardItem.CreatorUserId,
                                            LastModificationTime = rewardItem.LastModificationTime,
                                            LastModifierUserId= rewardItem.LastModifierUserId
                                        }).ToListAsync();

            var rewardBranchDtos = await (
                                         from branch in _dbContext.Branches
                                         join area in _dbContext.Areas on branch.AreaId equals area.Id into areaLeft
                                         from area in areaLeft.DefaultIfEmpty()
                                         join zone in _dbContext.Zones on branch.ZoneId equals zone.Id into zoneLeft
                                         from zone in zoneLeft.DefaultIfEmpty()
                                         join rewardBranch in _dbContext.RewardBranches on new { BranchId = branch.Id, RewardPackageId = entity.Id } equals new     { BranchId = rewardBranch.BranchId, rewardBranch.RewardPackageId } into rewardBranchLeft
                                         from rewardBranch in rewardBranchLeft.DefaultIfEmpty()
                                         where branch.IsActive
                                         select new RewardBranchDto()
                                         {
                                             Id = rewardBranch.Id,
                                             BranchId = branch.Id,
                                             BranchCode = branch.Code,
                                             BranchName = branch.Name,
                                             IsSelected = rewardBranch != null,
                                             ZoneName = zone.Name,
                                             AreaName = area.Name,
                                             ZoneId = branch.ZoneId,
                                             AreaId = branch.AreaId

                                         }).ToListAsync();

            var entityDto = Mapper.Map<RewardPackageDto>(entity);
            entityDto.RewardItems = rewardItemDtos;
            entityDto.Code = entity.Code;
            entityDto.Name = entity.Name;
            entityDto.IsActive = entity.IsActive;
            entityDto.Type = (int)entity.Type;
            entityDto.RewardBranches = rewardBranchDtos;
            entityDto.ToDate = entity.ToDate;
            entityDto.FromDate = entity.FromDate;
            entityDto.TotalTickets = entity.TotalTickets;
            entityDto.TotalAmount = entity.TotalAmount;
            return entityDto;
        }
    }
}