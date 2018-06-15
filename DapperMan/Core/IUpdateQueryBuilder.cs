namespace DapperMan.Core
{
    /// <summary>
    /// Build a query to update data.
    /// </summary>
    public interface IUpdateQueryBuilder : ICacheableNonQuery
    {
        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// The IUpdateQueryBuilder instance.
        /// </returns>
        IUpdateQueryBuilder Where(string filter);
    }
}
