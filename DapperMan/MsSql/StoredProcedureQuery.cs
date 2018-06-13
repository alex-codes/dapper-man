using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class StoredProcedureQuery : DapperQueryBase, IStoredProcedureQueryBuilder
    {
        //TODO: is there a way to easily handle output parameters?

        protected List<string> Parameters { get; private set; } = new List<string>();
        protected string ProcedureName { get; private set; }

        public StoredProcedureQuery(string procedureName, string connectionString)
            : this(procedureName, connectionString, null)
        {
        }

        public StoredProcedureQuery(string procedureName, string connectionString, int? commandTimeout)
           : base(connectionString)
        {
            CommandTimeout = commandTimeout;
            ProcedureName = procedureName;
        }

        public StoredProcedureQuery(string procedureName, IDbConnection connection)
            : this(procedureName, connection, null)
        {
        }

        public StoredProcedureQuery(string procedureName, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            CommandTimeout = commandTimeout;
            ProcedureName = procedureName;
        }

        public virtual int Execute(object queryParameters = null, IDbTransaction transaction = null)
        {
            return base.Execute(ProcedureName, queryParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public virtual async Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null)
        {
            int result = await base.ExecuteAsync(ProcedureName, queryParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            return result;
        }

        public virtual (IEnumerable<T> Results, int ReturnValue) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            var results = Query<T>(ProcedureName, queryParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            return (results, results.Count());
        }

        public virtual async Task<(IEnumerable<T> Results, int ReturnValue)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
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
                .Replace("  ", "");

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
