using Cbms.Dependency;
using Cbms.Kms.Domain.OracleProvider;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using System.Data;

namespace Cbms.Kms.Infrastructure.OracleProvider
{
    public class OracleConnectionOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Sid { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class OracleDataAccess : IOracleDataAccess, ITransientDependency
    {
        private readonly OracleConnectionOptions _options;

        public OracleDataAccess(OracleConnectionOptions options, ILogger logger)
        {
            _options = options; // méo et nên no snull nhứ bỏ mấy cái comment linh tinh nha :)
            logger.Information(options.ToString());
        }

        private OracleConnection GetConnection()
        {
            // 'Connection String' kết nối trực tiếp tới Oracle.
            string connString = $"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = {_options.Host})(PORT = {_options.Port}))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                 + _options.Sid + ")));Password=" + _options.Password + ";User ID=" + _options.User;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = connString;
            return conn;
        }

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (OracleConnection connection = GetConnection())
            {
                connection.Open();

                OracleCommand command = new OracleCommand(query, connection);
                command.CommandTimeout = 0;
                command.BindByName = true;
                //command.CommandType = CommandType.StoredProcedure;
                command.CommandType = CommandType.Text;
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.Add(item, parameter[i]);

                            i++;
                        }
                    }
                }

                OracleDataAdapter adapter = new OracleDataAdapter(command);

                adapter.Fill(data);

                connection.Close();
            }

            return data;
        }
    }
}