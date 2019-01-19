using Dapper;
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
    /// Build a query to select a single page of data from a table.
    /// </summary>
    public class PageableSelectQuery : SelectQuery
    {
        private string defaultQueryTemplate = "SELECT * FROM {source} {filter} {sort} OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY;";
        private string defaultCountQueryTemplate = "SELECT COUNT(*) FROM {source} {filter};";

        /// <summary>
        /// The number of rows to skip.
        /// </summary>
        protected int Skip { get; set; }

        /// <summary>
        /// The size of the page to take.
        /// </summary>
        protected int Take { get; set; }

        /// <summary>
        /// Creates a new paged select query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public PageableSelectQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        /// <summary>
        /// Creates a new paged select query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public PageableSelectQuery(string source, string connectionString, int? commandTimeout)
            : base(source, connectionString, commandTimeout)
        {

        }

        /// <summary>
        /// Creates a new paged select query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public PageableSelectQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        /// <summary>
        /// Creates a new paged select query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public PageableSelectQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {

        }

        /// <summary>
        /// Creates a new paged select query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        internal PageableSelectQuery(string source, string connectionString, IDbConnection connection, int? commandTimeout)
            : base(source, connectionString, commandTimeout)
        {
            Connection = connection;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable representing a single page of data and the total number of rows that match the query.
        /// </returns>
        public override (IEnumerable<T> Results, int TotalRows) Execute<T>(object queryParameters = null, IDbTransaction transaction = null)
        {
            IEnumerable<T> results = null;
            int totalRows = 0;

            void mapper(SqlMapper.GridReader gridReader)
            {
                results = gridReader.Read<T>();
                totalRows = gridReader.Read<int>().First();
            }

            QueryMultiple(GenerateStatement(), mapper, queryParameters, transaction: transaction);

            return (results, totalRows);
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable representing a single page of data and the total number of rows that match the query.
        /// </returns>
        public override async Task<(IEnumerable<T> Results, int TotalRows)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null)
        {
            IEnumerable<T> results = null;
            int totalRows = 0;

            void mapper(SqlMapper.GridReader gridReader)
            {
                results = gridReader.Read<T>();
                totalRows = gridReader.Read<int>().First();
            }

            await QueryMultipleAsync(GenerateStatement(), mapper, queryParameters, transaction: transaction);

            return (results, totalRows);
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public override string GenerateStatement()
        {
            if (string.IsNullOrWhiteSpace(Source))
            {
                throw new ArgumentNullException(nameof(Source));
            }

            if (SortOrders.Count == 0)
            {
                throw new ArgumentException("You must provide a sort order prior to selecting a page of data.");
            }

            string filter = string.Join(" AND ", Filters);
            string sort = string.Join(", ", SortOrders);
            int offset = Skip;
            int pageSize = Take;

            string sql = defaultQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .Replace("{sort}", string.IsNullOrWhiteSpace(sort) ? "" : "ORDER BY " + sort)
                .Replace("{offset}", offset.ToString())
                .Replace("{pageSize}", pageSize.ToString())
                .TrimEmptySpace();

            string rowCountSql = defaultCountQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .TrimEmptySpace();

            sql += rowCountSql;

            Debug.WriteLine(sql);

            return sql;
        }

        /// <summary>
        /// Sets the page size and page offset for the query.
        /// </summary>
        /// <param name="skip">The number of rows to skip.</param>
        /// <param name="take">The number of rows to take.</param>
        /// <returns>
        /// The instance of ISelectQueryBuilder.
        /// </returns>
        public override ISelectQueryBuilder SkipTake(int skip, int take)
        {
            if (skip < 0 || take < 0)
            {
                throw new ArgumentException("Cannot skip/take less than 0 records");
            }

            Skip = skip;
            Take = take;

            return this;
        }
    }
}
