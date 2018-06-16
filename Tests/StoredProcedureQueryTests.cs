using System;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class StoredProcedureQueryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcedureQuery_Execute_ThrowsWhenProcedureNameNull()
        {
            DapperQuery.StoredProcedure(null, "constr")
                .Execute();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcedureQuery_ExecuteT_ThrowsWhenProcedureNameNull()
        {
            DapperQuery.StoredProcedure(null, "constr")
                .Execute<MockData.TableNoIdentity>();
        }
    }
}
