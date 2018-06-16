using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    /// <summary>
    /// Build a query to count rows in a table.
    /// </summary>
    public class CountQuery : DapperQueryBase, ICountQueryBuilder, IQueryGenerator
    {
        private string defaultQueryTemplate = "SELECT [Count] = COUNT(*) FROM {source} {filter};";

        /// <summary>
        /// The list of filter strings to apply to the query.
        /// </summary>
        protected List<string> Filters { get; private set; } = new List<string>();

        /// <summary>
        /// Creates a new count query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public CountQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        /// <summary>
        /// Creates a new count query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public CountQuery(string source, string connectionString, int? commandTimeout)
            : base(connectionString)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Creates a new count query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database.</param>
        public CountQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        /// <summary>
        /// Creates a new count query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public CountQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The count of rows that match the criteria.
        /// </returns>
        public virtual int Execute(object queryParameters = null, IDbTransaction transaction = null)
        {
            return Query<int>(GenerateStatement(), queryParameters, transaction: transaction).First();
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The count of rows that match the criteria.
        /// </returns>
        public virtual async Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null)
        {
            var results = await QueryAsync<int>(GenerateStatement(), queryParameters, transaction: transaction);
            return results.First();
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public virtual string GenerateStatement()
        {
            string filter = string.Join(" AND ", Filters);

            string sql = defaultQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .TrimEmptySpace();

            Debug.WriteLine(sql);

            return sql;
        }

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// The ICountQueryBuilder instance.
        /// </returns>
        public virtual ICountQueryBuilder Where(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                throw new ArgumentNullException(nameof(filter));
            }

            Filters.Add(filter);
            return this;
        }
    }
}
