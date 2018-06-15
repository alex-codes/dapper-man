namespace DapperMan.Core
{
    /// <summary>
    /// Generates a sql statement.
    /// </summary>
    public interface IQueryGenerator
    {
        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns></returns>
        string GenerateStatement();
    }
}
