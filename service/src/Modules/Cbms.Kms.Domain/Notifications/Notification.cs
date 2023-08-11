using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Notifications.Actions;
using Cbms.Kms.Domain.Staffs;
using Cbms.Timing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Notifications
{
    public class Notification : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public NotificationStatus Status { get; private set; }
        public NotificationObjectType ObjectType { get; private set; }
        public string Description { get; private set; }
        public string ShortContent { get; private set; }
        public string Content { get; private set; }
        public bool IsSystem { get; private set; }
        public IReadOnlyCollection<NotificationUser> NotificationUsers => _notificationUsers;
        public List<NotificationUser> _notificationUsers = new List<NotificationUser>();

        public IReadOnlyCollection<NotificationBranch> NotificationBranches => _notificationBranches;
        public List<NotificationBranch> _notificationBranches = new List<NotificationBranch>();

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case NotificationSendAction sendAction:
                    await SendAsync(sendAction);
                    break;

                case NotificationGenerateUserAction generateAction:
                    await GenerateDetailAsync(generateAction);
                    break;

                case NotificationCreateDetailFromUserAction createDetailFromUserAction:
                    await CreateDetailFromUserAsync(createDetailFromUserAction);
                    break;

                case NotificationUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;

                case NotificationCompleteAction completeAction:
                    await CompleteAsync(completeAction);
                    break;

                case NotificationDeleteAction deleteAction:
                    await DeleteAsync(deleteAction);
                    break;
            }
        }

        private async Task DeleteAsync(NotificationDeleteAction action)
        {
            if (Status != NotificationStatus.Holding)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Notification.DeleteInvalid", Code).Build();
            }
        }

        private async Task CompleteAsync(NotificationCompleteAction action)
        {
            Status = NotificationStatus.Sended;
        }

        private async Task SendAsync(NotificationSendAction action)
        {
            if (Status != NotificationStatus.Holding)
            {
                return;
            }
            Status = NotificationStatus.Sending;
            await action.IocResolver.Resolve<INotificationSender>().SendSync(Id);
        }

        private async Task GenerateDetailAsync(NotificationGenerateUserAction action)
        {
            var staffFinder = action.IocResolver.Resolve<IStaffUserFinder>();
            var branchIds = NotificationBranches.Select(p => p.BranchId).ToList();
            List<int> userIds = new List<int>();
            if (ObjectType == NotificationObjectType.Sales)
            {
                userIds = (await staffFinder.FindByBranchAsync(branchIds)).Where(p=>p.UserId.HasValue).Select(p => p.UserId.Value).ToList();
            }
            else if (ObjectType == NotificationObjectType.Shop)
            {
                var customerRepository = action.IocResolver.Resolve<IRepository<Customer, int>>();

                userIds =  customerRepository.GetAll()
                        .Where(p => p.BranchId != null
                            && branchIds.Contains(p.BranchId.Value)
                            && p.UserId != null).Select(p => p.UserId.Value).ToList();
                    
            }
            foreach (var id in userIds)
            {
                var notificationUser = new NotificationUser();
                await notificationUser.ApplyActionAsync(new NotificationUserCreateAction(id));

                _notificationUsers.Add(notificationUser);
            }
        }

        private async Task CreateDetailFromUserAsync(NotificationCreateDetailFromUserAction action)
        {
            foreach (var id in action.Users)
            {
                var notificationUser = new NotificationUser();
                await notificationUser.ApplyActionAsync(new NotificationUserCreateAction(id));

                _notificationUsers.Add(notificationUser);
            }
        }

        private async Task UpsertAsync(NotificationUpsertAction action)
        {
            if (!action.IsNew && Status != NotificationStatus.Holding)
            {
                return;
            }

            if (action.IsNew)
            {
                Code = "TB" + Clock.Now.ToString("YYMMddHHmmss");
            }
            IsSystem = action.IsSystem;
            ObjectType = action.ObjectType;
            Description = action.Description;
            ShortContent = action.ShortContent;
            Content = action.Content;
            Status = NotificationStatus.Holding;

            foreach (var branchId in action.DeleteBranches)
            {
                var notificationBranch = _notificationBranches.FirstOrDefault(p => p.BranchId == branchId);
                if (notificationBranch != null)
                {
                    _notificationBranches.Remove(notificationBranch);
                }
            }

            foreach (var branchId in action.UpsertBranches)
            {
                var notificationBranch = _notificationBranches.FirstOrDefault(p => p.BranchId == branchId);
                if (notificationBranch == null)
                {
                    notificationBranch = new NotificationBranch();
                    _notificationBranches.Add(notificationBranch);
                }

                await notificationBranch.ApplyActionAsync(new NotificationBranchUpsertAction(branchId));
            }
        }
    }
}