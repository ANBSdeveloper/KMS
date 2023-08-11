using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.UserSalesOrgs.Actions
{
    public class UpsertUserAssignmentAction : IEntityAction
    {
        public int UserId { get; private set; }
        public int SalesOrgId { get; private set; }

        public UpsertUserAssignmentAction(int userId, int salesOrgId)
        {
            UserId = userId;
            SalesOrgId = salesOrgId;
        }
    }
}
