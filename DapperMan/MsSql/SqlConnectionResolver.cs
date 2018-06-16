using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    internal static class SqlConnectionResolver
    {
        public static object Connection { get; private set; }

        internal static IDbConnection ResolveConnection(string connectionString, IDbConnection connection, bool autoOpen = true)
        {
            if (connection != null)
            {
                if (connection.State != ConnectionState.Open && autoOpen)
                {
                    connection.Open();
                }

                return connection;
            }

            var conn = new SqlConnection(connectionString);

            if (conn.State != ConnectionState.Open && autoOpen)
            {
                conn.Open();
            }

            return conn;
        }

        internal static async Task<IDbConnection> ResolveConnectionAsync(string connectionString, IDbConnection connection, bool autoOpen = true)
        {
            if (connection != null)
            {
                if (connection.State != ConnectionState.Open && autoOpen)
                {
                    connection.Open();
                }

                return connection;
            }

            var conn = new SqlConnection(connectionString);

            if (conn.State != ConnectionState.Open && autoOpen)
            {
                await conn.OpenAsync();
            }

            return conn;
        }
    }
}
