using Cbms.Domain.Entities;
using System;

namespace Cbms.Kms.Domain.Products.Actions
{
    public class UpsertProductAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public string Unit { get; private set; }
        public string CaseUnit { get; private set; }
        public int PackSize { get; private set; }
        public int? ProductClassId { get; private set; }
        public int? SubProductClassId { get; private set; }
        public int? BrandId { get; private set; }
        public DateTime UpdateDate { get; private set; }

        public UpsertProductAction(
            string code,
            string name,
            string description,
            string unit,
            string caseUnit,
            int packSize,
            int? productClassId,
            int? subProductClassId,
            int? brandId,
            DateTime updateDate,
            bool isActive)
        {
            CaseUnit = caseUnit;
            Code = code;
            IsActive  = isActive;
            Name = name;
            Description = description ?? "";
            PackSize = packSize;
            Unit = unit;
            UpdateDate = updateDate;
            ProductClassId = productClassId;
            SubProductClassId = subProductClassId;
            BrandId = brandId;
        }
    }
}