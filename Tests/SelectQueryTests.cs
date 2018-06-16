using System;
using DapperMan.Core;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class SelectQueryTests
    {
        [TestMethod]
        public void SelectQuery_GenerateStatement()
        {
            string sql = @"
SELECT *
FROM TableName
WHERE A = @a
AND B = @b
ORDER BY C, D;";

            string expected = sql
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Select("TableName", "constr")
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
            ((IQueryGenerator)DapperQuery.Select(null, "constr"))
                .GenerateStatement();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelctQuery_SkipTake_ThrowsWhenSkipNegative()
        {
            DapperQuery.Select(null, "constr")
                .SkipTake(-1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectQuery_SkipTake_ThrowsWhenTakeNegative()
        {
            DapperQuery.Select(null, "constr")
                .SkipTake(0, -1);
        }

        [TestMethod]
        public void SelectQuery_SkipTake_NewPageableQuery()
        {
            var query = DapperQuery.Select("TableName", "constr")
                .SkipTake(0, 0);

            Assert.IsTrue(query is PageableSelectQuery);
        }

        [TestMethod]
        public void SelectQuery_SkipTake_GeneratePageableStatement()
        {
            var query = DapperQuery.Select("TableName", "constr")
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
ORDER BY C, D";

            string expected = sql
                .Replace(Environment.NewLine, " ")
                .Trim();

            string actual = ((IQueryGenerator)query).GenerateStatement();
            Assert.IsTrue(((IQueryGenerator)query).GenerateStatement().Contains(expected));
        }
    }
}
