using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Cbms.Kms.Domain.Notifications;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Notifications.Dto
{
    [AutoMap(typeof(Notification))]
    public class NotificationDto : NotificationBaseDto
    {
        [Ignore]
        public List<NotificationBranchDto> NotificationBranches { get; set; }
    }
}