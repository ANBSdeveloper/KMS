using Cbms.Domain.Entities;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.Notifications.Actions
{
    public class NotificationUpsertAction : IEntityAction
    {
        public bool IsNew { get; private set; }
        public NotificationObjectType ObjectType { get; private set; }
        public string Description { get; private set; }
        public string ShortContent { get; private set; }
        public string Content { get; private set; }
        public bool IsSystem { get; private set; }
        public List<int> UpsertBranches { get; set; }
        public List<int> DeleteBranches { get; set; }

        public NotificationUpsertAction(
            bool isNew,
            NotificationObjectType objectType,
            string description,
            string shortContent,
            string content,
            bool isSystem,
            List<int> upsertBranches,
            List<int> deleteBranches)
        {
            IsNew = isNew;
            IsSystem = isSystem;
            ObjectType = objectType;
            Description = description;
            ShortContent = shortContent;
            Content = content;
            UpsertBranches = upsertBranches;
            DeleteBranches = deleteBranches;
        }
    }
}