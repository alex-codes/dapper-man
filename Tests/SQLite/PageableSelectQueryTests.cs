using System;
using System.Data;
using DapperMan.Core;
using DapperMan.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.SQLite
{
    [TestClass]
    public class PageableSelectQueryTests
    {
        [TestMethod]
        public void PageableSelectQuery_GenerateStatement()
        {
            var conn = new Mock<IDbConnection>().Object;

            string sql = @"
SELECT *
FROM TableName
WHERE A = @a
AND B = @b
ORDER BY C, D
LIMIT 10 OFFSET 25;";

            string count = @"
SELECT COUNT(*) as [Count]
FROM TableName
WHERE A = @a
AND B = @b;";

            string expected = sql
                .Replace(Environment.NewLine, " ")
                .Trim()
                + count
                    .Replace(Environment.NewLine, " ")
                    .Trim();

            var query = DapperQuery.PageableSelect("TableName", conn)
                .Where("A = @a")
                .Where("B = @b")
                .OrderBy("C")
                .OrderBy("D")
                .SkipTake(25, 10);

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageableSelectQuery_GenerateStatement_ThrowsWhenNullSource()
        {
            var conn = new Mock<IDbConnection>().Object;

            ((IQueryGenerator)DapperQuery.PageableSelect(null, conn))
                .GenerateStatement();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PageableSelectQuery_GenerateStatement_ThrowsWhenNullSort()
        {
            var conn = new Mock<IDbConnection>().Object;

            ((IQueryGenerator)DapperQuery.PageableSelect("tablename", conn))
                .GenerateStatement();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PageableSelectQuery_SkipTake_ThrowsWhenSkipNegative()
        {
            var conn = new Mock<IDbConnection>().Object;

            DapperQuery.PageableSelect(null, conn)
                .SkipTake(-1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PageableSelectQuery_SkipTake_ThrowsWhenTakeNegative()
        {
            var conn = new Mock<IDbConnection>().Object;

            DapperQuery.PageableSelect(null, conn)
                .SkipTake(0, -1);
        }
    }
}
