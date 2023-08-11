using Cbms.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.PosmPrices.Actions
{
    public class PosmPriceUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public bool IsActive { get; private set; }
        public List<PosmPriceDetailUpsertAction> UpsertDetails { get; set; }
        public List<int> DeleteItems { get; set; }
      

        public PosmPriceUpsertAction(
            string code,
            string name,
            DateTime fromDate,
            DateTime toDate,
            bool isActive,
            List<PosmPriceDetailUpsertAction> upsertDetails,
            List<int> deleteDetails)
        {
            Code = code;
            Name = name;
            FromDate = fromDate;
            ToDate = toDate;
            UpsertDetails = upsertDetails;
            DeleteItems = deleteDetails;
            IsActive = isActive;
        }
    }
}