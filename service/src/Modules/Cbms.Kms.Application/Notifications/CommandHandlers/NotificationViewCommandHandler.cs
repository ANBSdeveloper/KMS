using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Notifications.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.Notifications.Actions;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications.CommandHandlers
{
    public class NotificationViewCommandHandler : CommandHandlerBase, IRequestHandler<NotificationViewCommand>
    {
        private readonly IRepository<NotificationUser, int> _notificationUserRepository;

        public NotificationViewCommandHandler(
            IRequestSupplement supplement,
            IRepository<NotificationUser, int> notificationUserRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _notificationUserRepository = notificationUserRepository;
        }

        public async Task<Unit> Handle(NotificationViewCommand request, CancellationToken cancellationToken)
        {
            var entity = await _notificationUserRepository.GetAll().FirstOrDefaultAsync(p => p.NotificationId == request.Id && p.UserId == Session.UserId);

            await entity.ApplyActionAsync(new NotificationUserViewAction());

            await _notificationUserRepository.UnitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}