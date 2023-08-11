using Cbms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Domain.SalesOrgs.Actions
{
    public class UpsertSalesOrgAction : IEntityAction
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int ParentId { get; private set; }
        public int TypeId { get; private set; }
        public string TypeName { get; private set; }
        public DateTime UpdateDate { get; private set; }


        public UpsertSalesOrgAction(int id, string code, string name, int parentId, int typeId, string typeName,DateTime updateDate)
        {
            Id = id;
            Code = code;
            Name = name;
            ParentId = parentId;
            TypeId = typeId;
            TypeName = typeName;
            UpdateDate = updateDate;
        }
    }
}
