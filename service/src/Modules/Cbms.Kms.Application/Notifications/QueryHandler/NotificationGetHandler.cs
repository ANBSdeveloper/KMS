using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Kms.Application.Notifications.Query;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications.QueryHandler
{
    public class NotificationGetHandler : QueryHandlerBase, IRequestHandler<NotificationGet, NotificationDto>
    {
        private readonly IRepository<Notification, int> _repository;
        private readonly AppDbContext _dbContext;

        public NotificationGetHandler(IRequestSupplement supplement, IRepository<Notification, int> repository, AppDbContext dbContext) : base(supplement)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<NotificationDto> Handle(NotificationGet request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAsync(request.Id);

            var notificationBranchDtos = await (
                                         from branch in _dbContext.Branches
                                         join area in _dbContext.Areas on branch.AreaId equals area.Id into areaLeft
                                         from area in areaLeft.DefaultIfEmpty()
                                         join zone in _dbContext.Zones on branch.ZoneId equals zone.Id into zoneLeft
                                         from zone in zoneLeft.DefaultIfEmpty()
                                         join notificationBranch in _dbContext.NotificationBranches on 
                                            new { BranchId = branch.Id, NotificationId = entity.Id } equals 
                                            new { BranchId = notificationBranch.BranchId, notificationBranch.NotificationId } into notificationBranchLeft
                                         from notificationBranch in notificationBranchLeft.DefaultIfEmpty()
                                         where branch.IsActive
                                         select new NotificationBranchDto()
                                         {
                                             Id = notificationBranch.Id,
                                             BranchId = branch.Id,
                                             BranchCode = branch.Code,
                                             BranchName = branch.Name,
                                             IsSelected = notificationBranch != null,
                                             ZoneName = zone.Name,
                                             AreaName = area.Name,
                                             ZoneId = branch.ZoneId,
                                             AreaId = branch.AreaId,
                                             CreationTime = notificationBranch != null ? notificationBranch.CreationTime : default(DateTime),
                                             CreatorUserId = notificationBranch != null ? notificationBranch.CreatorUserId : null,
                                             LastModificationTime = notificationBranch != null ? notificationBranch.LastModificationTime : null,
                                             LastModifierUserId = notificationBranch != null ? notificationBranch.LastModifierUserId : null,
                                         }).ToListAsync();

            var entityDto = Mapper.Map<NotificationDto>(entity);

            entityDto.NotificationBranches = notificationBranchDtos;

            return entityDto;
        }
    }
}