using DapperMan.Core;
using System.Data;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public abstract class MsSqlQueryBase : SqlQueryBase
    {
        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public MsSqlQueryBase(string source, string connectionString)
            : this(source, connectionString, null)
        {
        }

        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public MsSqlQueryBase(string source, string connectionString, int? commandTimeout)
            : base(source, connectionString, commandTimeout)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public MsSqlQueryBase(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public MsSqlQueryBase(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        /// <param name="autoOpen">If true, the connection is opened prior to use.</param>
        /// <returns>
        /// An instance of <see cref="SqlConnection"/>
        /// </returns>
        protected override IDbConnection ResolveConnection(bool autoOpen = true)
        {
            return SqlConnectionResolver.ResolveConnection(ConnectionString, Connection, autoOpen);
        }

        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        /// <param name="autoOpen">If true, the connection is opened prior to use.</param>
        /// <returns>
        /// An instance of <see cref="SqlConnection"/>
        /// </returns>
        protected override Task<IDbConnection> ResolveConnectionAsync(bool autoOpen = true)
        {
            return SqlConnectionResolver.ResolveConnectionAsync(ConnectionString, Connection, autoOpen);
        }
    }
}
