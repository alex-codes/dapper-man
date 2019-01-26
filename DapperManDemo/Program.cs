using System;

namespace DapperManDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            RunSqliteDemo();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void RunSqlDemo()
        {
            var demo = new SqlClientDemo();
            demo.ReadAllDepartments();
            demo.FindDepartment();
            demo.DepartmentExists();
            demo.ReadDepartmentIds();
        }

        static void RunSqliteDemo()
        {
            var demo = new SqliteDemo();
            demo.RunAll();
        }
    }
}
