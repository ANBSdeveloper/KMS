using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Notifications.Commands;
using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Kms.Application.Notifications.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.Notifications.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications.CommandHandlers
{
    public class NotificationSendCommandHandler : UpsertEntityCommandHandler<NotificationSendCommand, NotificationGet, NotificationDto>
    {
        private readonly IRepository<Notification, int> _notificationRepository;

        public NotificationSendCommandHandler(
            IRequestSupplement supplement,
            IRepository<Notification, int> notificationRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _notificationRepository = notificationRepository;
        }

        protected override async Task<NotificationDto> HandleCommand(NotificationSendCommand request, CancellationToken cancellationToken)
        {
            var entity = await _notificationRepository.GetAsync(request.Data.Id);

            await entity.ApplyActionAsync(new NotificationSendAction(IocResolver, LocalizationSource));

            await _notificationRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}