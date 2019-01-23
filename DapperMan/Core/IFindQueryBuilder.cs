using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    //TODO: should find query throw/return a flag if more than one result is found?

    /// <summary>
    /// Build a query to select a single row from a table.
    /// </summary>
    public interface IFindQueryBuilder
    {
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// A single object that matches the search criteria.
        /// </returns>
        T Execute<T>(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// A single object that matches the search criteria.
        /// </returns>
        Task<T> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// The IFindQueryBuilder instance.
        /// </returns>
        IFindQueryBuilder Where(string filter);
    }
}
