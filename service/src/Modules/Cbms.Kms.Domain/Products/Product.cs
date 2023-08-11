using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Products.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Products
{
    public class Product : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public string Unit { get; private  set; }
        public string CaseUnit { get; private set; }
        public int PackSize { get; private set; }
        public int? ProductClassId { get; private set; }
        public int? SubProductClassId { get; private set; }
        public int? BrandId { get; private set; }
        public DateTime UpdateDate { get; private set; }
        private Product()
        {
        }

        public static Product Create()
        {
            return new Product();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertProductAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(UpsertProductAction action)
        {
            BrandId = action.BrandId;
            ProductClassId = action.ProductClassId;
            SubProductClassId = action.SubProductClassId;
            CaseUnit = action.CaseUnit;
            Code = action.Code;
            IsActive = action.IsActive;
            Name = action.Name;
            Description = action.Description ?? "";
            PackSize = action.PackSize;
            Unit = action.Unit;
            UpdateDate = action.UpdateDate;
        }
    }
}