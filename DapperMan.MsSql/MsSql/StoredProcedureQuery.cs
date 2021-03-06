﻿using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    /// <summary>
    /// Run a stored procedure.
    /// </summary>
    public class StoredProcedureQuery : MsSqlQueryBase, IStoredProcedureQueryBuilder
    {
        //TODO: is there a way to easily handle output parameters?

        /// <summary>
        /// The list of query parameters.
        /// </summary>
        protected List<string> Parameters { get; private set; } = new List<string>();

        /// <summary>
        /// The stored procedure name.
        /// </summary>
        protected string ProcedureName { get; private set; }

        /// <summary>
        /// Creates a new stored procedure query.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="connectionString">The connection string to the database.</param>
        public StoredProcedureQuery(string procedureName, string connectionString)
            : this(procedureName, connectionString, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public StoredProcedureQuery(string procedureName, string connectionString, int? commandTimeout)
           : base(null, connectionString, commandTimeout)
        {
            ProcedureName = procedureName;
        }

        /// <summary>
        /// Creates a new stored procedure query.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public StoredProcedureQuery(string procedureName, IDbConnection connection)
            : this(procedureName, connection, null)
        {
        }

        /// <summary>
        /// Creates a new stored procedure query.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public StoredProcedureQuery(string procedureName, IDbConnection connection, int? commandTimeout)
            : base(null, connection, commandTimeout)
        {
            ProcedureName = procedureName;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An int representing the result of the stored procedure.
        /// </returns>
        public virtual int Execute(object queryParameters = null, IDbTransaction transaction = null)
        {
            if (string.IsNullOrWhiteSpace(ProcedureName))
            {
                throw new ArgumentNullException(nameof(ProcedureName));
            }

            return base.ExecuteNonQuery(ProcedureName, queryParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// An int representing the result of the stored procedure.
        /// </returns>
        public virtual async Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null)
        {
            if (string.IsNullOrWhiteSpace(ProcedureName))
            {
                throw new ArgumentNullException(nameof(ProcedureName));
            }

            int result = await base.ExecuteNonQueryAsync(ProcedureName, queryParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            return result;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns IEnumerable and count of total rows.
        /// </returns>
        public virtual (IEnumerable<T> Results, int ReturnValue) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(ProcedureName))
            {
                throw new ArgumentNullException(nameof(ProcedureName));
            }

            var results = Query<T>(ProcedureName, queryParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            return (results, results.Count());
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// Returns IEnumerable and count of total rows.
        /// </returns>
        public virtual async Task<(IEnumerable<T> Results, int ReturnValue)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(ProcedureName))
            {
                throw new ArgumentNullException(nameof(ProcedureName));
            }

            var results = await QueryAsync<T>(ProcedureName, queryParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            return (results, results.Count());
        }

        /*
        private string FormatParameter(string parameter, ParameterDirection direction)
        {
            if (!parameter.StartsWith("@"))
            {
                parameter = "@" + parameter;
            }

            return parameter + (direction == ParameterDirection.Output ? " OUTPUT" : "");
        }

        public virtual string GenerateStatement()
        {
            string parameters = string.Join(", ", Parameters);

            string sql = defaultQueryTemplate
                .Replace("{source}", ProcedureName)
                .Replace("{params}", parameters)
                .TrimEmptySpace();

            Debug.WriteLine(sql);

            return sql;
        }

        public IStoredProcedureQueryBuilder WithParameter(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            Parameters.Add(FormatParameter(parameter, ParameterDirection.Input));
            return this;
        }
        */
    }
}
