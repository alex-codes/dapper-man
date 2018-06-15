using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    /// <summary>
    /// A class containing all base functionality for query builders.
    /// </summary>
    public abstract class DapperQueryBase
    {
        /// <summary>
        /// Number of seconds before command execution timeout.
        /// </summary>
        protected int? CommandTimeout { get; set; }

        /// <summary>
        /// The connection string to the database.
        /// </summary>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// A connection to the database.
        /// </summary>
        protected IDbConnection Connection { get; set; }

        /// <summary>
        /// The name and schema of the table.
        /// </summary>
        protected string Source { get; set; }

        /// <summary>
        /// Create a new instance of DapperQueryBase.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        protected DapperQueryBase(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            ConnectionString = connectionString;
        }

        /// <summary>
        /// Create a new instance of DapperQueryBase.
        /// </summary>
        /// <param name="connection">A connection to the database.</param>
        protected DapperQueryBase(IDbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The result from executing the query, typically the number of rows affected.
        /// </returns>
        public virtual int Execute(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = ResolveConnection())
            {
                return conn.Execute(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The result from executing the query, typically the number of rows affected.
        /// </returns>
        public async virtual Task<int> ExecuteAsync(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = await ResolveConnectionAsync())
            {
                return await conn.ExecuteAsync(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public virtual IEnumerable<T> Query<T>(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = ResolveConnection())
            {
                return conn.Query<T>(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public async virtual Task<IEnumerable<T>> QueryAsync<T>(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = await ResolveConnectionAsync())
            {
                return await conn.QueryAsync<T>(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">A function used to map the results of SqlMapper.GridReader.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        public virtual void QueryMultiple(string query, Action<SqlMapper.GridReader> map, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = ResolveConnection())
            {
                var results = conn.QueryMultiple(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
                map(results);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">A function used to map the results of SqlMapper.GridReader.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>A task.</returns>
        public async virtual Task QueryMultipleAsync(string query, Action<SqlMapper.GridReader> map, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = await ResolveConnectionAsync())
            {
                var results = await conn.QueryMultipleAsync(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
                map(results);
            }
        }

        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        /// <param name="autoOpen">If true, the connection is opened prior to use.</param>
        /// <returns>
        /// An instance of <see cref="SqlConnection"/>
        /// </returns>
        protected virtual IDbConnection ResolveConnection(bool autoOpen = true)
        {
            if (Connection != null)
            {
                return Connection;
            }

            var conn = new SqlConnection(ConnectionString);
            conn.Open();

            return conn;
        }

        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        /// <param name="autoOpen">If true, the connection is opened prior to use.</param>
        /// <returns>
        /// An instance of <see cref="SqlConnection"/>
        /// </returns>
        protected async virtual Task<IDbConnection> ResolveConnectionAsync(bool autoOpen = true)
        {
            if (Connection != null)
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                return Connection;
            }

            var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync();

            return conn;
        }
    }
}
