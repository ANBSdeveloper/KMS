using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.Notifications.Dto
{
    public class NotificationUserListDto : EntityDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string ShortContent { get; set; }
        public string Content { get; set; }
        public DateTime? ViewDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}