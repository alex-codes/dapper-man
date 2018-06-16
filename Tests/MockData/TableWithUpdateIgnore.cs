using DapperMan.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.MockData
{
    public class TableWithUpdateIgnore
    {
        public string Field1 { get; set; }

        [UpdateIgnore]
        public string Field2 { get; set; }
    }
}
