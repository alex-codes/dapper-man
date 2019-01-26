using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperMan.SQLite
{
    /// <summary>
    /// Abstract class containing logic for SQLite.
    /// </summary>
    public abstract class SQLiteQueryBase : SqlQueryBase
    {
        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public SQLiteQueryBase(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public SQLiteQueryBase(string source, IDbConnection connection, int? commandTimeout)
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
        /// The provided instance of <see cref="IDbConnection"/>
        /// </returns>
        protected override IDbConnection ResolveConnection(bool autoOpen = true)
        {
            if (Connection.State != ConnectionState.Open && autoOpen)
            {
                Connection.Open();
            }

            return Connection;
        }

        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        /// <param name="autoOpen">If true, the connection is opened prior to use.</param>
        /// <returns>
        /// The provided instance of <see cref="IDbConnection"/>
        /// </returns>
        protected override Task<IDbConnection> ResolveConnectionAsync(bool autoOpen = true)
        {
            if (Connection.State != ConnectionState.Open && autoOpen)
            {
                Connection.Open();
            }

            return Task.FromResult(Connection);
        }
    }
}
