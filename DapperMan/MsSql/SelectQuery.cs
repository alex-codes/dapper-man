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
    /// Build a query to select data from a table.
    /// </summary>
    public class SelectQuery : DapperQueryBase, ISelectQueryBuilder, IQueryGenerator
    {
        private readonly string defaultQueryTemplate = "SELECT * FROM {source} {filter} {sort};";

        /// <summary>
        /// The list of filter strings to apply to the query.
        /// </summary>
        protected List<string> Filters { get; private set; } = new List<string>();

        /// <summary>
        /// The list of sort order strings to apply to the query.
        /// </summary>
        protected List<string> SortOrders { get; private set; } = new List<string>();

        /// <summary>
        /// Creates a new select query
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public SelectQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        /// <summary>
        /// Creates a new select query
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public SelectQuery(string source, string connectionString, int? commandTimeout)
            : base(connectionString)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Creates a new select query
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database.</param>
        public SelectQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        /// <summary>
        /// Creates a new select query
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public SelectQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns IEnumerable and count of total rows.
        /// </returns>
        public virtual (IEnumerable<T> Results, int TotalRows) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            var results = Query<T>(GenerateStatement(), queryParameters, transaction: transaction);
            return (results, results.Count());
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns IEnumerable and count of total rows.
        /// </returns>
        public virtual async Task<(IEnumerable<T> Results, int TotalRows)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            var results = await QueryAsync<T>(GenerateStatement(), queryParameters, transaction: transaction);
            return (results, results.Count());
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
            string sort = string.Join(", ", SortOrders);

            string sql = defaultQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .Replace("{sort}", string.IsNullOrWhiteSpace(sort) ? "" : "ORDER BY " + sort)
                .Replace("  ", "");

            Debug.WriteLine(sql);

            return sql;
        }

        /// <summary>
        /// Adds a sort order to the query.
        /// </summary>
        /// <param name="orderBy">The column name to order by.</param>
        /// <returns>
        /// This ISelectQueryBuilder instance.
        /// </returns>
        public virtual ISelectQueryBuilder OrderBy(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            SortOrders.Add(orderBy);
            return this;
        }

        /// <summary>
        /// Adds paging to the query.
        /// </summary>
        /// <param name="skip">The number of rows to skip.</param>
        /// <param name="take">The number of rows to take.</param>
        /// <returns>
        /// A new instance if ISelectQueryBuilder.
        /// </returns>
        public virtual ISelectQueryBuilder SkipTake(int skip, int take)
        {
            var query = new PageableSelectQuery(Source, ConnectionString, Connection, CommandTimeout);

            foreach (string filter in Filters)
            {
                query.Where(filter);
            }

            foreach (string sort in SortOrders)
            {
                query.OrderBy(sort);
            }

            return query.SkipTake(skip, take);
        }

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// This ISelectQueryBuilder instance.
        /// </returns>
        public virtual ISelectQueryBuilder Where(string filter)
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
