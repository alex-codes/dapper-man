using DapperMan.Core;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    /// <summary>
    /// Builds a query to delete data from a table.
    /// </summary>
    public class DeleteQuery : MsSqlQueryBase, IDeleteQueryBuilder, IQueryGenerator
    {
        private readonly string defaultQueryTemplate = "DELETE FROM {source} {filter};";

        /// <summary>
        /// Creates a new delete query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public DeleteQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        /// <summary>
        /// Creates a new delete query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public DeleteQuery(string source, string connectionString, int? commandTimeout)
            : base(source, connectionString, commandTimeout)
        {
        }

        /// <summary>
        /// Creates a new delete query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public DeleteQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        /// <summary>
        /// Creates a new delete query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public DeleteQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affescted.
        /// </returns>
        public virtual int Execute(object queryParameters = null, IDbTransaction transaction = null)
        {
            return ExecuteNonQuery(GenerateStatement(), queryParameters, transaction: transaction);
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affescted.
        /// </returns>
        public virtual async Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null)
        {
            return await ExecuteNonQueryAsync(GenerateStatement(), queryParameters, transaction: transaction);
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public virtual string GenerateStatement()
        {
            if (string.IsNullOrWhiteSpace(Source))
            {
                throw new ArgumentNullException(nameof(Source));
            }

            string filter = string.Join(" AND ", Filters);

            string sql = this.defaultQueryTemplate
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
        /// The IDeleteQueryBuilder instance.
        /// </returns>
        public virtual IDeleteQueryBuilder Where(string filter)
        {
            AddFilter(filter);
            return this;
        }
    }
}
