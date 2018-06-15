using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    /// <summary>
    /// Build a query to select data from a table.
    /// </summary>
    public interface ISelectQueryBuilder
    {
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns IEnumerable and count of total rows.
        /// </returns>
        (IEnumerable<T> Results, int TotalRows) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns IEnumerable and count of total rows.
        /// </returns>
        Task<(IEnumerable<T> Results, int TotalRows)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Adds a sort order to the query.
        /// </summary>
        /// <param name="orderBy">The column name to order by.</param>
        /// <returns>
        /// This ISelectQueryBuilder instance.
        /// </returns>
        ISelectQueryBuilder OrderBy(string orderBy);

        /// <summary>
        /// Adds paging to the query.
        /// </summary>
        /// <param name="skip">The number of rows to skip.</param>
        /// <param name="take">The number of rows to take.</param>
        /// <returns>
        /// A new instance if ISelectQueryBuilder.
        /// </returns>
        ISelectQueryBuilder SkipTake(int skip, int take);

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// This ISelectQueryBuilder instance.
        /// </returns>
        ISelectQueryBuilder Where(string filter);
    }
}
