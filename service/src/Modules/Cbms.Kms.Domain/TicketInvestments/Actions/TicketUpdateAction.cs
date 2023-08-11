using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketUpdateAction : IEntityAction
    {
        public int? StaffId { get; private set; }

        public TicketUpdateAction(int? staffId)
        {
            StaffId = staffId;
        }
    }
}