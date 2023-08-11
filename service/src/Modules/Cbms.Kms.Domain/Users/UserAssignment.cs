using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.UserSalesOrgs.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Users
{
    public class UserAssignment: AuditedAggregateRoot
    {
        public int UserId { get; private set; }
        public int SalesOrgId { get; private set; }

        public UserAssignment()
        {

        }

        public static UserAssignment Create()
        {
            return new UserAssignment();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertUserAssignmentAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertUserAssignmentAction action)
        {
            UserId = action.UserId;
            SalesOrgId = action.SalesOrgId;
        }
    }
}
