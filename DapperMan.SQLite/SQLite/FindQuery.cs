using DapperMan.Core;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.SQLite
{
    /// <summary>
    /// Build a query to select a single row from a table.
    /// </summary>
    public class FindQuery : SQLiteQueryBase, IFindQueryBuilder, IQueryGenerator
    {
        private readonly string defaultQueryTemplate = "SELECT * FROM {source} {filter} LIMIT 1;";

        /// <summary>
        /// Creates a new select query that returns a single result
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public FindQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        /// <summary>
        /// Creates a new select query that returns a single result
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public FindQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// A single object that matches the search criteria.
        /// </returns>
        public T Execute<T>(object queryParameters = null, IDbTransaction transaction = null)
        {
            var results = Query<T>(GenerateStatement(), queryParameters, transaction: transaction);
            return results.FirstOrDefault();
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// A single object that matches the search criteria.
        /// </returns>
        public async Task<T> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null)
        {
            var results = await QueryAsync<T>(GenerateStatement(), queryParameters, transaction: transaction);
            return results.FirstOrDefault();
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public string GenerateStatement()
        {
            if (string.IsNullOrWhiteSpace(Source))
            {
                throw new ArgumentNullException(nameof(Source));
            }

            string filter = string.Join(" AND ", Filters);
            string sort = string.Join(", ", SortOrders);

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
        /// <returns>This IFindQueryBuilder instance.</returns>
        public IFindQueryBuilder Where(string filter)
        {
            AddFilter(filter);
            return this;
        }
    }
}
