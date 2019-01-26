using System;
using System.Data;
using DapperMan.Core;
using DapperMan.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.SQLite
{
    [TestClass]
    public class FindQueryTests
    {
        [TestMethod]
        public void FindQuery_GenerateStatement()
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

            var query = DapperQuery.Find("dbo.TableName", conn)
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            var conn = new Mock<IDbConnection>().Object;

            ((IQueryGenerator)DapperQuery.Find(null, conn))
                .GenerateStatement();
        }
    }
}
