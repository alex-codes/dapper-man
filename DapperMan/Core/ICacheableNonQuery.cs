using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    /// <summary>
    /// Build a query that caches information about the typed parameter.
    /// </summary>
    public interface ICacheableNonQuery
    {
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        int Execute<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        Task<int> ExecuteAsync<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class;
    }
}
