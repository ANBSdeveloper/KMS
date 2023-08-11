using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Notifications.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.Notifications.Actions;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications.CommandHandlers
{
    public class NotificationDeleteCommandHandler : DeleteEntityCommandHandler<NotificationDeleteCommand, Notification>
    {
        private readonly IRepository<Notification, int> _notificationRepository;

        public NotificationDeleteCommandHandler(IRequestSupplement supplement, IRepository<Notification, int> notificationRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _notificationRepository = notificationRepository;
        }

        public async override Task<Unit> Handle(NotificationDeleteCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetAsync(request.Id);
            await notification.ApplyActionAsync(new NotificationDeleteAction(LocalizationSource));
            return await base.Handle(request, cancellationToken);
        }
    }
}