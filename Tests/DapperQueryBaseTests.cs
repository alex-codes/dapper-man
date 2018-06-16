using System;
using System.Data;
using System.Threading.Tasks;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DapperQueryBaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DapperQueryBase_ThrowsWhenConnectionStringNull()
        {
            new FakeQuery(connectionString: null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DapperQueryBase_ThrowsWhenConnectionNull()
        {
            new FakeQuery(connection: null);
        }
    }

    public class FakeQuery : DapperQueryBase
    {
        public FakeQuery(string connectionString) : base(connectionString)
        {
        }

        public FakeQuery(IDbConnection connection) : base(connection)
        {
        }

        protected override IDbConnection ResolveConnection(bool autoOpen = true)
        {
            throw new NotImplementedException();
        }

        protected override Task<IDbConnection> ResolveConnectionAsync(bool autoOpen = true)
        {
            throw new NotImplementedException();
        }
    }
}
