using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Notifications.Commands;
using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Kms.Application.Notifications.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.Notifications.Actions;
using Cbms.Mediator;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications.CommandHandlers
{
    public class NotificationUpsertCommandHandler : UpsertEntityCommandHandler<NotificationUpsertCommand, NotificationGet, NotificationDto>
    {
        private readonly IRepository<Notification, int> _notificationRepository;

        public NotificationUpsertCommandHandler(
            IRequestSupplement supplement,
            IRepository<Notification, int> rewardPackageRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;

            _notificationRepository = rewardPackageRepository;
        }

        protected override async Task<NotificationDto> HandleCommand(NotificationUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            Notification entity = null;
            bool isNew = false;
            if (!request.Data.Id.IsNew())
            {
                entity = await _notificationRepository
                    .GetAllIncluding(p => p.NotificationBranches)
                    .FirstOrDefaultAsync(p => p.Id == entityDto.Id);

                if (entity == null)
                {
                    throw new EntityNotFoundException(typeof(Notification), entityDto.Id);
                }
            }
            else
            {
                isNew = true;
                entity = new Notification();
                await _notificationRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new NotificationUpsertAction(
                isNew,
                (NotificationObjectType)entityDto.ObjectType,
                entityDto.Description,
                entityDto.ShortContent,
                entityDto.Content,
                false,
                entityDto.NotificationBranchChanges.UpsertedItems.Select(p => p.BranchId).ToList(),
                entityDto.NotificationBranchChanges.DeletedItems.Select(p => p.BranchId).ToList()
            ));

            await _notificationRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}