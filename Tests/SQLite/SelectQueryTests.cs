using System;
using System.Data;
using DapperMan.Core;
using DapperMan.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.SQLite
{
    [TestClass]
    public class SelectQueryTests
    {
        [TestMethod]
        public void SelectQuery_GenerateStatement()
        {
            var conn = new Mock<IDbConnection>().Object;

            string sql = @"
SELECT *
FROM TableName
WHERE A = @a
AND B = @b
ORDER BY C, D;";

            string expected = sql
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Select("TableName", conn)
                .Where("A = @a")
                .Where("B = @b")
                .OrderBy("C")
                .OrderBy("D");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SelectQuery_GenerateStatement_ThrowsWhenNullSource()
        {
            var conn = new Mock<IDbConnection>().Object;

            ((IQueryGenerator)DapperQuery.Select(null, conn))
                .GenerateStatement();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelctQuery_SkipTake_ThrowsWhenSkipNegative()
        {
            var conn = new Mock<IDbConnection>().Object;

            DapperQuery.Select(null, conn)
                .SkipTake(-1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectQuery_SkipTake_ThrowsWhenTakeNegative()
        {
            var conn = new Mock<IDbConnection>().Object;

            DapperQuery.Select(null, conn)
                .SkipTake(0, -1);
        }

        [TestMethod]
        public void SelectQuery_SkipTake_NewPageableQuery()
        {
            var conn = new Mock<IDbConnection>().Object;

            var query = DapperQuery.Select("TableName", conn)
                .SkipTake(0, 0);

            Assert.IsTrue(query is PageableSelectQuery);
        }

        [TestMethod]
        public void SelectQuery_SkipTake_GeneratePageableStatement()
        {
            var conn = new Mock<IDbConnection>().Object;

            var query = DapperQuery.Select("TableName", conn)
                .Where("A = @a")
                .Where("B = @b")
                .OrderBy("C")
                .OrderBy("D")
                .SkipTake(0, 0);

            string sql = @"
SELECT *
FROM TableName
WHERE A = @a
AND B = @b
ORDER BY C, D
LIMIT 0 OFFSET 0";

            string expected = sql
                .Replace(Environment.NewLine, " ")
                .Trim();

            string actual = ((IQueryGenerator)query).GenerateStatement();
            Assert.IsTrue(((IQueryGenerator)query).GenerateStatement().Contains(expected));
        }
    }
}
