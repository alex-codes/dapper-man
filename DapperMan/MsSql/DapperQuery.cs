using DapperMan.Core;
using System.Data;

namespace DapperMan.MsSql
{
    public class DapperQuery : DapperQueryBase
    {
        public DapperQuery(string connectionString)
            : base(connectionString)
        {

        }

        public DapperQuery(IDbConnection connection)
            : base(connection)
        {

        }

        // static factory methods for a cleaner builder pattern
        public static DapperQuery Create(string connectionString)
        {
            return new DapperQuery(connectionString);
        }

        public static DapperQuery Create(IDbConnection connection)
        {
            return new DapperQuery(connection);
        }

        public static IDeleteQueryBuilder Delete(string source, string connectionString)
        {
            return new DeleteQuery(source, connectionString);
        }

        public static IDeleteQueryBuilder Delete(string source, string connectionString, int? commandTimeout)
        {
            return new DeleteQuery(source, connectionString, commandTimeout);
        }

        public static IDeleteQueryBuilder Delete(string source, IDbConnection connection)
        {
            return new DeleteQuery(source, connection);
        }

        public static IDeleteQueryBuilder Delete(string source, IDbConnection connection, int? commandTimeout)
        {
            return new DeleteQuery(source, connection, commandTimeout);
        }

        public static IInsertQueryBuilder Insert(string source, string connectionString)
        {
            return new InsertQuery(source, connectionString);
        }

        public static IInsertQueryBuilder Insert(string source, string connectionString, int? commandTimeout)
        {
            return new InsertQuery(source, connectionString, commandTimeout);
        }

        public static IInsertQueryBuilder Insert(string source, IDbConnection connection)
        {
            return new InsertQuery(source, connection);
        }

        public static IInsertQueryBuilder Insert(string source, IDbConnection connection, int? commandTimeout)
        {
            return new InsertQuery(source, connection, commandTimeout);
        }

        public static ISelectQueryBuilder PageableSelect(string source, string connectionString)
        {
            return new PageableSelectQuery(source, connectionString);
        }

        public static ISelectQueryBuilder PageableSelect(string source, string connectionString, int? commandTimeout)
        {
            return new PageableSelectQuery(source, connectionString, commandTimeout);
        }

        public static ISelectQueryBuilder PageableSelect(string source, IDbConnection connection)
        {
            return new PageableSelectQuery(source, connection);
        }

        public static ISelectQueryBuilder PageableSelect(string source, IDbConnection connection, int? commandTimeout)
        {
            return new PageableSelectQuery(source, connection, commandTimeout);
        }

        public static ISelectQueryBuilder Select(string source, string connectionString)
        {
            return new SelectQuery(source, connectionString);
        }

        public static ISelectQueryBuilder Select(string source, string connectionString, int? commandTimeout)
        {
            return new SelectQuery(source, connectionString, commandTimeout);
        }

        public static ISelectQueryBuilder Select(string source, IDbConnection connection)
        {
            return new SelectQuery(source, connection);
        }

        public static ISelectQueryBuilder Select(string source, IDbConnection connection, int? commandTimeout)
        {
            return new SelectQuery(source, connection, commandTimeout);
        }

        public static IUpdateQueryBuilder Update(string source, string connectionString)
        {
            return new UpdateQuery(source, connectionString);
        }

        public static IUpdateQueryBuilder Update(string source, string connectionString, int? commandTimeout)
        {
            return new UpdateQuery(source, connectionString, commandTimeout);
        }

        public static IUpdateQueryBuilder Update(string source, IDbConnection connection)
        {
            return new UpdateQuery(source, connection);
        }

        public static IUpdateQueryBuilder Update(string source, IDbConnection connection, int? commandTimeout)
        {
            return new UpdateQuery(source, connection, commandTimeout);
        }
    }
}
