using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.SubProductClasses.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.SubProductClasses
{
    public class SubProductClass : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public SubProductClass()
        {
        }

      
        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case SubProductClassUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(SubProductClassUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
        }
    }
}