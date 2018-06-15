using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    /// <summary>
    /// Run a stored procedure.
    /// </summary>
    public interface IStoredProcedureQueryBuilder
    {
        /// <summary>
        /// Executes the query
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An int representing the result of the stored procedure.
        /// </returns>
        int Execute(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An int representing the result of the stored procedure.
        /// </returns>
        Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null);

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns IEnumerable and count of total rows.
        /// </returns>
        (IEnumerable<T> Results, int ReturnValue) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns IEnumerable and count of total rows.
        /// </returns>
        Task<(IEnumerable<T> Results, int ReturnValue)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class;
    }
}
