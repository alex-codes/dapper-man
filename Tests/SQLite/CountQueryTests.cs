using System;
using System.Data;
using DapperMan.Core;
using DapperMan.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.SQLite
{
    [TestClass]
    public class CountQueryTests
    {
        [TestMethod]
        public void CountQuery_GenerateStatement()
        {
            var conn = new Mock<IDbConnection>().Object;

            string expected = @"
SELECT COUNT(*) as [Count]
FROM dbo.TableName
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Count("dbo.TableName", conn)
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            var conn = new Mock<IDbConnection>().Object;

            ((IQueryGenerator)DapperQuery.Count(null, conn))
                .GenerateStatement();
        }
    }
}
