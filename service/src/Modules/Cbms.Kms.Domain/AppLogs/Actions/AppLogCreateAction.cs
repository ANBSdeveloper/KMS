using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.AppLogs.Actions
{
    public class AppLogCreateAction : IEntityAction
    {
        public LogType Type { get; private set; }
        public string Name { get; private set; }
        public string Data { get; private set; }

        public AppLogCreateAction(LogType type, string name, string data)
        {
            Type = type;
            Name = name;
            Data = data;
        }
    }
}