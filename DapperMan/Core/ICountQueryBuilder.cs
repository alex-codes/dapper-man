using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    /// <summary>
    /// Build a query to count rows in a table.
    /// </summary>
    public interface ICountQueryBuilder
    {
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The count of rows that match the criteria.
        /// </returns>
        int Execute(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The count of rows that match the criteria.
        /// </returns>
        Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// The IUpdateQueryBuilder instance.
        /// </returns>
        ICountQueryBuilder Where(string filter);
    }
}
