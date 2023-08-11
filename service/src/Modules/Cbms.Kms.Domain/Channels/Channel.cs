using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Channels.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Channels
{
    public class Channel : AuditedAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public int SalesOrgId { get; private set; }

        public Channel()
        {
        }

        public static Channel Create()
        {
            return new Channel();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertChannelsAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertChannelsAction action)
        {
            Code = action.Code;
            Name = action.Name;
            SalesOrgId = action.SalesOrgId;
        }
    }
}