using Cbms.Dto;

namespace Cbms.Kms.Application.Notifications.Dto
{
    public class NotificationUpsertDto : EntityDto
    {
        public string Description { get; set; }
        public string Content { get; set; }
        public string ShortContent { get; set; }
        public int ObjectType { get; set; }
        public CrudListDto<NotificationBranchUpsertDto> NotificationBranchChanges { get; set; }

        public NotificationUpsertDto()
        {
            NotificationBranchChanges = new CrudListDto<NotificationBranchUpsertDto>();
        }
    }
}