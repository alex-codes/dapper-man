using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    /// <summary>
    /// Builds a query to determine if a record exists.
    /// </summary>
    public interface IExistsQueryBuilder
    {
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns true if the record exists; otherwise, false.
        /// </returns>
        bool Execute(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns true if the record exists; otherwise, false.
        /// </returns>
        Task<bool> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// The IFindQueryBuilder instance.
        /// </returns>
        IExistsQueryBuilder Where(string filter);
    }
}
