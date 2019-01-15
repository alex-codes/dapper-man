using DapperMan.Core;
using DapperMan.Core.Attributes;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    /// <summary>
    /// Build a query to insert data into a table.
    /// </summary>
    public class InsertQuery : SqlQueryBase, IInsertQueryBuilder, IGenericQueryGenerator
    {
        private string defaultQyeryTemplate = "INSERT INTO {source} ({fields}) VALUES ({values});";
        private string keyField = null;
        private string[] propNames = null;
        private string scopeIdentityTemplate = "SELECT CAST(SCOPE_IDENTITY() as int);";

        /// <summary>
        /// Creates a new insert query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public InsertQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {
        }

        /// <summary>
        /// Creates a new insert query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public InsertQuery(string source, string connectionString, int? commandTimeout)
           : base(source, connectionString, commandTimeout)
        {
        }

        /// <summary>
        /// Creates a new insert query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database.</param>
        public InsertQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        /// <summary>
        /// Creates a new insert query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public InsertQuery(string source, IDbConnection connection, int? commandTimeout)
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
        /// The number of rows affescted.
        /// </returns>
        public virtual int Execute<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            string sql = GenerateStatement<T>(propertyCache);

            if (UseIdentity(queryParameters))
            {
                return Query<int>(sql, queryParameters, transaction: transaction).First();
            }
            else
            {
                return ExecuteNonQuery(sql, queryParameters, transaction: transaction);
            }
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
            string sql = GenerateStatement<T>(propertyCache);

            if (UseIdentity(queryParameters))
            {
                var result = await QueryAsync<int>(sql, queryParameters, transaction: transaction);
                return result.First();
            }
            else
            {
                return await ExecuteNonQueryAsync(sql, queryParameters, transaction: transaction);
            }
        }

        /// <summary>
        /// Formats property names for the update statement.
        /// </summary>
        /// <param name="props"></param>
        /// <returns>
        /// A string representing all formatted properties of the table.
        /// </returns>
        protected virtual string FormatPropertyNames(string[] props)
        {
            return string.Join(", ", props.Select(s => $"[{s}]"));
        }

        /// <summary>
        /// Formats parameter names for the update statement.
        /// </summary>
        /// <param name="props"></param>
        /// <returns>
        /// A string representing all formatted parameters of the query.
        /// </returns>
        protected virtual string FormatPropertyParameters(string[] props)
        {
            return string.Join(", ", props.Select(s => $"@{s}"));
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

            string sql = defaultQyeryTemplate
                .Replace("{source}", Source)
                .Replace("{fields}", FormatPropertyNames(propNames))
                .Replace("{values}", FormatPropertyParameters(propNames))
                .TrimEmptySpace();

            if (UseIdentity())
            {
                sql += scopeIdentityTemplate;
            }

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
            propNames = ReflectionHelper.ReflectProperties<T>(propertyCache, new[] { typeof(IdentityAttribute), typeof(InsertIgnoreAttribute) });
            keyField = ReflectionHelper.GetIdentityField<T>(propertyCache);
        }

        /// <summary>
        /// Determines whether or not to use the identity specification.
        /// </summary>
        /// <returns>
        /// A boolean that indicates whether or not to use the identity specification.
        /// </returns>
        private bool UseIdentity()
        {
            return !string.IsNullOrWhiteSpace(keyField);
        }

        /// <summary>
        /// Determines whether or not to use the identity specification.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <returns>
        /// A boolean that indicates whether or not to use the identity specification.
        /// </returns>
        private bool UseIdentity(object queryParameters)
        {
            return UseIdentity()
                && (queryParameters == null || queryParameters is string || !(queryParameters is IEnumerable));
        }
    }
}
