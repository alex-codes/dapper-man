using DapperMan.Core;
using DapperMan.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class UpdateQuery : DapperQueryBase, IUpdateQueryBuilder
    {
        private int? commandTimeout = null;
        private string defaultQyeryTemplate = "UPDATE {source} SET {fields} {filter};";
        protected List<string> Filters { get; private set; } = new List<string>();
        private string[] propNames = null;

        public UpdateQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {
        }

        public UpdateQuery(string source, string connectionString, int? commandTimeout)
           : base(connectionString)
        {
            this.commandTimeout = commandTimeout;
            Source = source;
        }

        public UpdateQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        public UpdateQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            this.commandTimeout = commandTimeout;
            Source = source;
        }

        public int Execute<T>(object queryParameters = null, ObjectCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            ReflectType<T>(propertyCache);
            return Execute(GenerateStatement(), queryParameters, transaction: transaction);
        }

        public async Task<int> ExecuteAsync<T>(object queryParameters = null, ObjectCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            ReflectType<T>(propertyCache);
            return await ExecuteAsync(GenerateStatement(), queryParameters, transaction: transaction);
        }

        protected string FormatPropertyNames(string[] props)
        {
            string propNames = "";

            for (int i = 0; i < props.Length; i++)
            {
                propNames = string.Concat(propNames, string.Format("{0} = @{0}", props[i]));

                if (i != props.Length - 1)
                {
                    propNames = string.Concat(propNames, ", ");
                }
            }

            return propNames;
        }

        public virtual string GenerateStatement()
        {
            string filter = string.Join(" AND ", Filters);

            string sql = defaultQyeryTemplate
                .Replace("{source}", Source)
                .Replace("{fields}", FormatPropertyNames(propNames))
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter);

            Debug.WriteLine(sql);

            return sql;
        }

        private void ReflectType<T>(ObjectCache propertyCache) where T : class
        {
            propNames = ReflectionHelper.ReflectProperties<T>(propertyCache, new[] { typeof(IdentityAttribute), typeof(UpdateIgnoreAttribute) });
        }

        public IUpdateQueryBuilder Where(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                throw new ArgumentNullException(nameof(filter));
            }

            Filters.Add(filter);
            return this;
        }
    }
}
