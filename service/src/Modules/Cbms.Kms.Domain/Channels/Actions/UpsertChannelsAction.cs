using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Channels.Actions
{
    public class UpsertChannelsAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int SalesOrgId { get; private set; }

        public UpsertChannelsAction(string code, string name, int salesOrgId)
        {
            Code = code;
            Name = name;
            SalesOrgId = salesOrgId;
        }
    }
}