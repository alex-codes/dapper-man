using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DapperMan.Tests")]
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
        /// A flag to indicate whether or not to keep the connection open upon completion of the query.
        /// </summary>
        public bool KeepAlive { get; protected set; }

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
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        protected DapperQueryBase(IDbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            KeepAlive = true;
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
        public virtual int ExecuteNonQuery(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = connFactory.ResolveConnection();
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
        public async virtual Task<int> ExecuteNonQueryAsync(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = await connFactory.ResolveConnectionAsync();
                return await conn.ExecuteAsync(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public virtual IEnumerable<T> Query<T>(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = connFactory.ResolveConnection();
                return conn.Query<T>(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T1">The first type of the record set.</typeparam>
        /// <typeparam name="T2">The second type of the record set.</typeparam>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="splitOn">The field used to split the results in order to map the second object.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public virtual IEnumerable<TResult> Query<T1, T2, TResult>(string query,
            Func<T1, T2, TResult> map,
            string splitOn = "Id",
            object param = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = connFactory.ResolveConnection();
                return conn.Query(query, map, splitOn: splitOn, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T1">The first type of the record set.</typeparam>
        /// <typeparam name="T2">The second type of the record set.</typeparam>
        /// <typeparam name="T3">The third type of the record set.</typeparam>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="splitOn">The field used to split the results in order to map the second object.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public virtual IEnumerable<TResult> Query<T1, T2, T3, TResult>(string query,
            Func<T1, T2, T3, TResult> map,
            string splitOn = "Id",
            object param = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = connFactory.ResolveConnection();
                return conn.Query(query, map, splitOn: splitOn, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T1">The first type of the record set.</typeparam>
        /// <typeparam name="T2">The second type of the record set.</typeparam>
        /// <typeparam name="T3">The third type of the record set.</typeparam>
        /// <typeparam name="T4">The fourth type of the record set.</typeparam>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="splitOn">The field used to split the results in order to map the second object.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public virtual IEnumerable<TResult> Query<T1, T2, T3, T4, TResult>(string query,
            Func<T1, T2, T3, T4, TResult> map,
            string splitOn = "Id",
            object param = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = connFactory.ResolveConnection();
                return conn.Query(query, map, splitOn: splitOn, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T1">The first type of the record set.</typeparam>
        /// <typeparam name="T2">The second type of the record set.</typeparam>
        /// <typeparam name="T3">The third type of the record set.</typeparam>
        /// <typeparam name="T4">The fourth type of the record set.</typeparam>
        /// <typeparam name="T5">The fifth type of the record set.</typeparam>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="splitOn">The field used to split the results in order to map the second object.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public virtual IEnumerable<TResult> Query<T1, T2, T3, T4, T5, TResult>(string query,
            Func<T1, T2, T3, T4, T5, TResult> map,
            string splitOn = "Id",
            object param = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = connFactory.ResolveConnection();
                return conn.Query(query, map, splitOn: splitOn, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public async virtual Task<IEnumerable<T>> QueryAsync<T>(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = await connFactory.ResolveConnectionAsync();
                return await conn.QueryAsync<T>(sql: query, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T1">The first type of the record set.</typeparam>
        /// <typeparam name="T2">The second type of the record set.</typeparam>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="splitOn">The field used to split the results in order to map the second object.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public async virtual Task<IEnumerable<TResult>> QueryAsync<T1, T2, TResult>(string query,
            Func<T1, T2, TResult> map,
            string splitOn = "Id",
            object param = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = await connFactory.ResolveConnectionAsync();
                return await conn.QueryAsync(query, map, splitOn: splitOn, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T1">The first type of the record set.</typeparam>
        /// <typeparam name="T2">The second type of the record set.</typeparam>
        /// <typeparam name="T3">The third type of the record set.</typeparam>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="splitOn">The field used to split the results in order to map the second object.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public async virtual Task<IEnumerable<TResult>> QueryAsync<T1, T2, T3, TResult>(string query,
            Func<T1, T2, T3, TResult> map,
            string splitOn = "Id",
            object param = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = await connFactory.ResolveConnectionAsync();
                return await conn.QueryAsync(query, map, splitOn: splitOn, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T1">The first type of the record set.</typeparam>
        /// <typeparam name="T2">The second type of the record set.</typeparam>
        /// <typeparam name="T3">The third type of the record set.</typeparam>
        /// <typeparam name="T4">The fourth type of the record set.</typeparam>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="splitOn">The field used to split the results in order to map the second object.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public async virtual Task<IEnumerable<TResult>> QueryAsync<T1, T2, T3, T4, TResult>(string query,
            Func<T1, T2, T3, T4, TResult> map,
            string splitOn = "Id",
            object param = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = await connFactory.ResolveConnectionAsync();
                return await conn.QueryAsync(query, map, splitOn: splitOn, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="T1">The first type of the record set.</typeparam>
        /// <typeparam name="T2">The second type of the record set.</typeparam>
        /// <typeparam name="T3">The third type of the record set.</typeparam>
        /// <typeparam name="T4">The fourth type of the record set.</typeparam>
        /// <typeparam name="T5">The fifth type of the record set.</typeparam>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="query">The sql statement to execute.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="splitOn">The field used to split the results in order to map the second object.</param>
        /// <param name="param">An object to which the query parameters are mapped.</param>
        /// <param name="commandType">The sql command type.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An IEnumerable of data.
        /// </returns>
        public async virtual Task<IEnumerable<TResult>> QueryAsync<T1, T2, T3, T4, T5, TResult>(string query,
            Func<T1, T2, T3, T4, T5, TResult> map,
            string splitOn = "Id",
            object param = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction transaction = null)
        {
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = await connFactory.ResolveConnectionAsync();
                return await conn.QueryAsync(query, map, splitOn: splitOn, param: param, commandType: commandType, transaction: transaction, commandTimeout: CommandTimeout);
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
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = connFactory.ResolveConnection();
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
            using (var connFactory = new ConnectionFactory(this))
            {
                var conn = await connFactory.ResolveConnectionAsync();

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
        protected abstract IDbConnection ResolveConnection(bool autoOpen = true);

        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        /// <param name="autoOpen">If true, the connection is opened prior to use.</param>
        /// <returns>
        /// An instance of <see cref="SqlConnection"/>
        /// </returns>
        protected abstract Task<IDbConnection> ResolveConnectionAsync(bool autoOpen = true);

        /// <summary>
        /// An internal connection factory used for cleaning up connections.
        /// </summary>
        protected class ConnectionFactory : IDisposable
        {
            private IDbConnection connection;
            private DapperQueryBase instance;

            public ConnectionFactory(DapperQueryBase instance)
            {
                this.instance = instance;
            }

            public virtual IDbConnection ResolveConnection(bool autoOpen = true)
            {
                connection = instance.ResolveConnection(autoOpen);
                return connection;
            }

            public virtual async Task<IDbConnection> ResolveConnectionAsync(bool autoOpen = true)
            {
                connection = await instance.ResolveConnectionAsync(autoOpen);
                return connection;
            }

            public void Dispose()
            {
                if (!instance.KeepAlive)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }

                    connection.Dispose();
                    connection = null;
                }
            }
        }
    }
}
