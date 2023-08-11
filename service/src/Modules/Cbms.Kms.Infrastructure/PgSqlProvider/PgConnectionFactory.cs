using Cbms.Dependency;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Cbms.Kms.Infrastructure.PgSqlProvider
{
    public class PgConnectionFactory : IPgConnectionFactory, ITransientDependency
    {
        private IDbConnection _connection;
        private PqConnectionOptions _options;

        public PgConnectionFactory(PqConnectionOptions options)
        {
            _options = options;
        }

        public Task<IDbConnection> GetConnectionAsync()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_options.ConnectionString);
                _connection.Open();
            }

            return Task.FromResult(_connection);
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Dispose();
            }
        }
    }
}