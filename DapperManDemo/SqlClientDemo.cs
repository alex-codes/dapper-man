using DapperMan.MsSql;
using DapperManDemo.Models.AdventureWorks;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DapperManDemo
{
    // project requres the adventureworks 2017 (or above) database
    public class SqlClientDemo
    {
        private string connStr;

        public SqlClientDemo()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            connStr = configuration.GetConnectionString("MSSQL");
        }

        public void ReadAllDepartments()
        {
            (var depts, int count) = DapperQuery.Select("HumanResources.Department", connStr)
                .Execute<Department>();

            foreach (var dept in depts)
            {
                Console.WriteLine(dept.ToString());
            }

            Console.WriteLine();
            Console.WriteLine($"{count} department records found.");
            Console.WriteLine();
        }
    }
}
