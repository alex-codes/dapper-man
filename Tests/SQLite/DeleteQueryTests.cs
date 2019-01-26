using System;
using System.Data;
using DapperMan.Core;
using DapperMan.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.SQLite
{
    [TestClass]
    public class DeleteQueryTests
    {
        [TestMethod]
        public void DeleteQuery_GenerateStatement()
        {
            var conn = new Mock<IDbConnection>().Object;

            string expected = @"
DELETE FROM dbo.TableName
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Delete("dbo.TableName", conn)
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            var conn = new Mock<IDbConnection>().Object;

            ((IQueryGenerator)DapperQuery.Delete(null, conn))
                .GenerateStatement();
        }
    }
}
