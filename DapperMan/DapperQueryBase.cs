using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public abstract class DapperQueryBase
    {
        protected int? CommandTimeout { get; set; }
        protected string ConnectionString { get; set; }
        protected IDbConnection Connection { get; set; }
        protected string Source { get; set; }

        protected DapperQueryBase(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            ConnectionString = connectionString;
        }

        protected DapperQueryBase(IDbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public virtual int Execute(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = ResolveConnection())
            {
                return conn.Execute(sql: query, param: param, commandType: commandType, transaction: transaction);
            }
        }

        public async virtual Task<int> ExecuteAsync(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = await ResolveConnectionAsync())
            {
                return await conn.ExecuteAsync(sql: query, param: param, commandType: commandType, transaction: transaction);
            }
        }

        public virtual IEnumerable<T> Query<T>(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = ResolveConnection())
            {
                return conn.Query<T>(sql: query, param: param, commandType: commandType, transaction: transaction);
            }
        }

        public async virtual Task<IEnumerable<T>> QueryAsync<T>(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = await ResolveConnectionAsync())
            {
                return await conn.QueryAsync<T>(sql: query, param: param, commandType: commandType, transaction: transaction);
            }
        }

        public virtual SqlMapper.GridReader QueryMultiple(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = ResolveConnection())
            {
                return conn.QueryMultiple(sql: query, param: param, commandType: commandType, transaction: transaction);
            }
        }

        public async virtual Task<SqlMapper.GridReader> QueryMultipleAsync(string query, object param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            using (var conn = await ResolveConnectionAsync())
            {
                return await conn.QueryMultipleAsync(sql: query, param: param, commandType: commandType, transaction: transaction);
            }
        }

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
