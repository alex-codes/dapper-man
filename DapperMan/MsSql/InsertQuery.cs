using DapperMan.Core;
using DapperMan.Core.Attributes;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    /// <summary>
    /// Build a query to insert data into a table.
    /// </summary>
    public class InsertQuery : DapperQueryBase, IInsertQueryBuilder, IQueryGenerator
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
           : base(connectionString)
        {
            CommandTimeout = commandTimeout;
            Source = source;
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
            : base(connection)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affescted.
        /// </returns>
        public virtual int Execute<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            ReflectType<T>(propertyCache);

            if (!string.IsNullOrWhiteSpace(keyField))
            {
                return Query<int>(GenerateStatement(), queryParameters, transaction: transaction).First();
            }
            else
            {
                return ExecuteNonQuery(GenerateStatement(), queryParameters, transaction: transaction);
            }
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The number of rows affescted.
        /// </returns>
        public virtual async Task<int> ExecuteAsync<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            ReflectType<T>(propertyCache);

            if (!string.IsNullOrWhiteSpace(keyField))
            {
                var result = await QueryAsync<int>(GenerateStatement(), queryParameters, transaction: transaction);
                return result.First();
            }
            else
            {
                return await ExecuteNonQueryAsync(GenerateStatement(), queryParameters, transaction: transaction);
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
            return string.Join(",", props.Select(s => $"[{s}]"));
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
            return string.Join(",", props.Select(s => $"@{s}"));
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public virtual string GenerateStatement()
        {
            string sql = defaultQyeryTemplate
                .Replace("{source}", Source)
                .Replace("{fields}", FormatPropertyNames(propNames))
                .Replace("{values}", FormatPropertyParameters(propNames))
                .TrimEmptySpace();

            if (!string.IsNullOrWhiteSpace(keyField))
            {
                sql += scopeIdentityTemplate;
            }

            Debug.WriteLine(sql);

            return sql;
        }

        /// <summary>
        /// Uses reflection to determine information about the typed class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        private void ReflectType<T>(PropertyCache propertyCache) where T : class
        {
            propNames = ReflectionHelper.ReflectProperties<T>(propertyCache, new[] { typeof(IdentityAttribute), typeof(InsertIgnoreAttribute) });
            keyField = ReflectionHelper.GetIdentityField<T>(propertyCache);
        }
    }
}
