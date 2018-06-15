namespace DapperMan.Core
{
    /// <summary>
    /// Builds a query to delete data from a table.
    /// </summary>
    public interface IDeleteQueryBuilder : INonQuery
    {
        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// The IDeleteQueryBuilder instance.
        /// </returns>
        IDeleteQueryBuilder Where(string filter);
    }
}
