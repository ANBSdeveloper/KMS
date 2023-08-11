using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Cycles.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Cycles
{
    public class Cycle : AuditedAggregateRoot
    {
        public int Year { get; private set; }
        public string Number { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        private Cycle()
        {
        }

        public static Cycle Create()
        {
            return new Cycle();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertCycleAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertCycleAction action)
        {
            Year = action.Year;
            Number = action.Number;
            FromDate = action.FromDate;
            ToDate = action.ToDate;
            IsActive = action.IsActive;
        }
    }
}