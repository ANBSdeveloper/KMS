using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.SalesOrgs.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.SalesOrgs
{
    public class SalesOrg : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int ParentId { get; private set; }
        public int TypeId { get; private set; }
        public string TypeName { get; private set; }
        public DateTime UpdateDate { get; private set; }

        public SalesOrg()
        {
        }

        public static SalesOrg Create()
        {
            return new SalesOrg();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertSalesOrgAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertSalesOrgAction action)
        {
            Id = action.Id;
            Code = action.Code;
            Name = action.Name;
            ParentId = action.ParentId;
            TypeId = action.TypeId;
            TypeName = action.TypeName;
            UpdateDate = action.UpdateDate;
        }
    }
}
