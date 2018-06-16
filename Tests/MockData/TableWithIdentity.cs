using DapperMan.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.MockData
{
    public class TableWithIdentity
    {
        [Identity]
        public int Id { get; set; }

        public string Field1 { get; set; }
    }
}
