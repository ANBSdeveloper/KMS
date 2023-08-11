using System.Data;
using System.Threading.Tasks;

namespace Cbms.Kms.Infrastructure.PgSqlProvider
{

    public interface IPgConnectionFactory
    {
        Task<IDbConnection> GetConnectionAsync();
    }
}   