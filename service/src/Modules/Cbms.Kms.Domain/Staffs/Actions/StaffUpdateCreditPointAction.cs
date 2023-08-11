using Cbms.Domain.Entities;
using System;

namespace Cbms.Kms.Domain.Staffs.Actions
{
    public class StaffUpdateCreditPointAction : IEntityAction
    {
        public int CreditPoints { get; set; }

        public StaffUpdateCreditPointAction(int creditPoints)
        {
            CreditPoints = creditPoints;
        }
    }
}
