using System.Data;

namespace Cbms.Kms.Domain.OracleProvider
{
    public interface IOracleDataAccess
    {
        public DataTable ExecuteQuery(string query, object[] parameter = null);
    }
}