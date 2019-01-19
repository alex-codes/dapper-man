using System;

namespace DapperManDemo.Models.AdventureWorks
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public DateTime ModifiedDate { get; set; }

        public override string ToString()
        {
            return $"{DepartmentId}\t{Name}\t{GroupName}\t{ModifiedDate}";
        }
    }
}
