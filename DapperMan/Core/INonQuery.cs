using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    /// <summary>
    /// Builds a query that does not return a result set.
    /// </summary>
    public interface INonQuery
    {
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affescted.
        /// </returns>
        int Execute(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affescted.
        /// </returns>
        Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null);
    }
}
