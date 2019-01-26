using System;
using System.Data;
using DapperMan.Core;
using DapperMan.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.SQLite
{
    [TestClass]
    public class ExistsQueryTests
    {
        [TestMethod]
        public void ExistsQuery_GenerateStatement()
        {
            var conn = new Mock<IDbConnection>().Object;

            string expected = @"
SELECT *
FROM dbo.TableName
WHERE A = @a
AND B = @b
LIMIT 1;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Exists("dbo.TableName", conn)
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExistsQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            var conn = new Mock<IDbConnection>().Object;

            ((IQueryGenerator)DapperQuery.Exists(null, conn))
                .GenerateStatement();
        }
    }
}
