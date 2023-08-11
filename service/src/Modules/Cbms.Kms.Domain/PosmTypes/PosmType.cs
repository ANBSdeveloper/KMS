using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Entities;
using Cbms.Kms.Domain.PosmTypes.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.PosmTypes
{
    public class PosmType : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        private PosmType()
        {
        }

        public static PosmType Create()
        {
            return new PosmType()
            {
            };
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmTypeUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(PosmTypeUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
        }
    }
}
