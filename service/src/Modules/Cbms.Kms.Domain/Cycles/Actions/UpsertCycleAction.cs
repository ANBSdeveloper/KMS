using Cbms.Domain.Entities;
using System;

namespace Cbms.Kms.Domain.Cycles.Actions
{
    public class UpsertCycleAction : IEntityAction
    {
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }

        public int Year { get; private set; }

        public bool IsActive { get; private set; }
        public string Number { get; private set; }

        public UpsertCycleAction(int year, string number, DateTime fromDate, DateTime toDate, bool isActive)
        {
            Year = year;
            FromDate = fromDate;
            ToDate = toDate;
            IsActive = isActive;
            Number = number;
        }
    }
}