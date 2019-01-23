using DapperMan.Core;
using DapperMan.Core.Attributes;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DapperMan.SQLite
{
    /// <summary>
    /// Build a query to update data.
    /// </summary>
    public class UpdateQuery : SQLiteQueryBase, IUpdateQueryBuilder, IGenericQueryGenerator
    {
        private string defaultQyeryTemplate = "UPDATE {source} SET {fields} {filter};";
        private string[] propNames = null;

        /// <summary>
        /// Creates a new update query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public UpdateQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        /// <summary>
        /// Creates a new update query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public UpdateQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {
        }

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
        public virtual int Execute<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            ReflectType<T>(propertyCache);
            return ExecuteNonQuery(GenerateStatement(), queryParameters, transaction: transaction);
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affescted.
        /// </returns>
        public virtual async Task<int> ExecuteAsync<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            ReflectType<T>(propertyCache);
            return await ExecuteNonQueryAsync(GenerateStatement(), queryParameters, transaction: transaction);
        }

        /// <summary>
        /// Formats property names for the update statement.
        /// </summary>
        /// <param name="props">The list of property names to include in the sql statement.</param>
        /// <returns>
        /// A string representing all formatted properties of the table.
        /// </returns>
        protected virtual string FormatPropertyNames(string[] props)
        {
            string propNames = "";

            for (int i = 0; i < props.Length; i++)
            {
                propNames = string.Concat(propNames, string.Format("[{0}] = @{0}", props[i]));

                if (i != props.Length - 1)
                {
                    propNames = string.Concat(propNames, ", ");
                }
            }

            return propNames;
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public virtual string GenerateStatement()
        {
            if (string.IsNullOrWhiteSpace(Source))
            {
                throw new ArgumentNullException(nameof(Source));
            }

            string filter = string.Join(" AND ", Filters);

            string sql = defaultQyeryTemplate
                .Replace("{source}", Source)
                .Replace("{fields}", FormatPropertyNames(propNames))
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .TrimEmptySpace();

            Debug.WriteLine(sql);

            return sql;
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public string GenerateStatement<T>(PropertyCache propertyCache) where T : class
        {
            ReflectType<T>(propertyCache);
            return GenerateStatement();
        }

        /// <summary>
        /// Uses reflection to determine information about the typed class.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        private void ReflectType<T>(PropertyCache propertyCache) where T : class
        {
            propNames = ReflectionHelper.ReflectProperties<T>(propertyCache, new[] { typeof(IdentityAttribute), typeof(UpdateIgnoreAttribute) });
        }

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// The IUpdateQueryBuilder instance.
        /// </returns>
        public virtual IUpdateQueryBuilder Where(string filter)
        {
            AddFilter(filter);
            return this;
        }
    }
}
