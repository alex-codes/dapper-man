using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class PageableSelectQuery : SelectQuery
    {
        private int skip;
        private int take;

        public PageableSelectQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        public PageableSelectQuery(string source, string connectionString, int? commandTimeout)
            : base(source, connectionString, commandTimeout)
        {

        }

        public PageableSelectQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        public PageableSelectQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {

        }

        internal PageableSelectQuery(string source, string connectionString, IDbConnection connection, int? commandTimeout)
            : base(source, connectionString, commandTimeout)
        {
            Connection = connection;
        }

        public static new ISelectQueryBuilder Create(string source, string connectionString)
        {
            return Create(source, connectionString, null);
        }

        public static new ISelectQueryBuilder Create(string source, string connectionString, int? commandTimeout)
        {
            return new PageableSelectQuery(source, connectionString, commandTimeout);
        }

        public static new ISelectQueryBuilder Create(string source, IDbConnection connection)
        {
            return Create(source, connection, null);
        }

        public static new ISelectQueryBuilder Create(string source, IDbConnection connection, int? commandTimeout)
        {
            return new PageableSelectQuery(source, connection, commandTimeout);
        }

        public override int Execute(string query, object param = null, CommandType commandType = CommandType.Text, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public override Task<int> ExecuteAsync(string query, object param = null, CommandType commandType = CommandType.Text, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public override string GenerateStatement()
        {
            throw new NotImplementedException();
        }

        public override ISelectQueryBuilder SkipTake(int skip, int take)
        {
            this.skip = skip;
            this.take = take;

            return this;
        }
    }
}
