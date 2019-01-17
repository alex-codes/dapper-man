using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public abstract class SqlQueryBase : DapperQueryBase
    {
        /// <summary>
        /// The list of filter strings to apply to the query.
        /// </summary>
        protected List<string> Filters { get; private set; } = new List<string>();

        /// <summary>
        /// The list of sort order strings to apply to the query.
        /// </summary>
        protected List<string> SortOrders { get; private set; } = new List<string>();

        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public SqlQueryBase(string source, string connectionString)
            : this(source, connectionString, null)
        {
        }

        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public SqlQueryBase(string source, string connectionString, int? commandTimeout)
            : base(connectionString)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public SqlQueryBase(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        /// <summary>
        /// Creates a new sql query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public SqlQueryBase(string source, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        protected internal void AddFilter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                throw new ArgumentNullException(nameof(filter));
            }

            Filters.Add(filter);
        }

        /// <summary>
        /// Adds a sort order to the query.
        /// </summary>
        /// <param name="orderBy">The column name to order by.</param>
        protected internal void AddSort(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            SortOrders.Add(orderBy);
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
