using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Entities;
using Cbms.Kms.Domain.PosmClasses.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.PosmClasses
{
    public class PosmClass : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IncludeInfo { get; private set; }

        private PosmClass()
        {
        }

        public static PosmClass Create()
        {
            return new PosmClass()
            {
            };
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmClassUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(PosmClassUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IncludeInfo= action.IncludeInfo;
            IsActive = action.IsActive;
        }
    }
}
