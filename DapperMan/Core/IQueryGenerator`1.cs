namespace DapperMan.Core
{
    /// <summary>
    /// Generates a sql statement.
    /// </summary>
    public interface IGenericQueryGenerator : IQueryGenerator
    {
        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        string GenerateStatement<T>(PropertyCache propertyCache) where T : class;
    }
}
