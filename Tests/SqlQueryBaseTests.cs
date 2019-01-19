using System;
using System.Data;
using DapperMan.Core;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests.MockData;

namespace Tests
{
    [TestClass]
    public class SqlQueryBaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SqlQueryBase_AddFilter_ThrowsWhenNull()
        {
            new Mock<SqlQueryBase>(new object[] { "", "constr" })
                .Object
                .AddFilter(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SqlQueryBase_AddSort_ThrowsWhenNull()
        {
            new Mock<SqlQueryBase>(new object[] { "", "constr" })
                .Object
                .AddSort(null);
        }
    }
}
