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

        public void DepartmentExists()
        {
            Console.WriteLine("/// DepartmentExists ///");
            Console.WriteLine();

            bool result = DapperQuery.Exists("HumanResources.Department", connStr)
                .Where("DepartmentId = @id")
                .Execute(new { id = 1 });

            Console.WriteLine(result.ToString());
            Console.WriteLine();
        }

        public void FindDepartment()
        {
            Console.WriteLine("/// FindDepartment ///");
            Console.WriteLine();

            var dept = DapperQuery.Find("HumanResources.Department", connStr)
                .Where("DepartmentId = @id")
                .Execute<Department>(new { id = 1 });

            Console.WriteLine(dept.ToString());
            Console.WriteLine();
        }

        public void ReadAllDepartments()
        {
            Console.WriteLine("/// ReadAllDepartments ///");
            Console.WriteLine();

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
