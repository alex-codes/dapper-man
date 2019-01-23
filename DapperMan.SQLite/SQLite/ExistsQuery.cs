using DapperMan.Core;
using System.Data;
using System.Threading.Tasks;

namespace DapperMan.SQLite
{
    /// <summary>
    /// Builds a query to determine if a record exists.
    /// </summary>
    public class ExistsQuery : SQLiteQueryBase, IExistsQueryBuilder, IQueryGenerator
    {
        private FindQuery query;

        /// <summary>
        /// Creates a new ExistsQuery.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public ExistsQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        /// <summary>
        /// Creates a new ExistsQuery.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public ExistsQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {
            query = new FindQuery(source, connection, commandTimeout);
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns true if the record exists; otherwise, false.
        /// </returns>
        public virtual bool Execute(object queryParameters = null, IDbTransaction transaction = null)
        {
            var result = query.Execute<object>(queryParameters, transaction);
            return result != null;
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns true if the record exists; otherwise, false.
        /// </returns>
        public virtual async Task<bool> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null)
        {
            var result = await query.ExecuteAsync<object>(queryParameters, transaction);
            return result != null;
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public string GenerateStatement()
        {
            return query.GenerateStatement();
        }

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>This IFindQueryBuilder instance.</returns>
        public IExistsQueryBuilder Where(string filter)
        {
            query.Where(filter);
            return this;
        }
    }
}
