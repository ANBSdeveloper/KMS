using Cbms.Dependency;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Runtime.Connection;
using Cbms.Timing;
using Dapper;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.AppLogs
{
    public class AppLogger : IAppLogger, ITransientDependency
    {
        private ISqlConnectionFactory _connectionFactory;

        public AppLogger(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task LogErrorAsync(string name, object data)
        {
            await LogAsync(LogType.Error, name, data);
        }

        public async Task LogErrorAsync(string name, string data)
        {
            await LogAsync(LogType.Error, name, data);
        }

        public async Task LogInfoAsync(string name, object data)
        {
            await LogAsync(LogType.Info, name, data);
        }

        public async Task LogInfoAsync(string name, string data)
        {
            await LogAsync(LogType.Info, name, data);
        }

        private async Task LogAsync(LogType type, string name, object data)
        {
            using (var connection = await _connectionFactory.GetConnectionAsync())
            {
                string insertQuery = @"
                    INSERT INTO [dbo].[AppLogs]([CreationTime], [Name], [Type], [Data]) VALUES (@CreationTime, @Name, @Type, @Data)";

                string dataInfo = data == null ? "" : (data.GetType().Name == typeof(string).Name ? data.ToString() : JsonConvert.SerializeObject(data));
                await connection.ExecuteAsync(insertQuery, new
                {
                    name,
                    type,
                    data = dataInfo,
                    CreationTime = Clock.Now
                });
            }
        }
    }
}