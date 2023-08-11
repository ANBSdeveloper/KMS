using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.AppLogs.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.AppLogs
{
    public class AppLog : Entity, IHasCreationTime
    {
        public LogType Type { get; private set; }
        public string Name { get; private set; }
        public string Data { get; private set; }
        public DateTime CreationTime { get; set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case AppLogCreateAction createAction:
                    await CreateAsync(createAction);
                    break;
            }
        }

        private async Task CreateAsync(AppLogCreateAction action)
        {
            Type = action.Type;
            Name = action.Name;
            Data = action.Data;
        }
    }
}
