using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Notifications;

namespace Cbms.Kms.Application.Notifications.Dto
{
    [AutoMap(typeof(Notification))]
    public class NotificationBaseDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string ShortContent { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int ObjectType { get; set; }
    }
}