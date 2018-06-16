using System;
using DapperMan.Core;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PageableSelectQueryTests
    {
        [TestMethod]
        public void PageableSelectQuery_GenerateStatement()
        {
            string sql = @"
SELECT *
FROM TableName
WHERE A = @a
AND B = @b
ORDER BY C, D
OFFSET 25 ROWS FETCH NEXT 10 ROWS ONLY;";

            string count = @"
SELECT COUNT(*)
FROM TableName
WHERE A = @a
AND B = @b;";

            string expected = sql
                .Replace(Environment.NewLine, " ")
                .Trim()
                + count
                    .Replace(Environment.NewLine, " ")
                    .Trim();

            var query = DapperQuery.PageableSelect("TableName", "constr")
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
            ((IQueryGenerator)DapperQuery.PageableSelect(null, "constr"))
                .GenerateStatement();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PageableSelectQuery_GenerateStatement_ThrowsWhenNullSort()
        {
            ((IQueryGenerator)DapperQuery.PageableSelect("tablename", "constr"))
                .GenerateStatement();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PageableSelectQuery_SkipTake_ThrowsWhenSkipNegative()
        {
            DapperQuery.PageableSelect(null, "constr")
                .SkipTake(-1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PageableSelectQuery_SkipTake_ThrowsWhenTakeNegative()
        {
            DapperQuery.PageableSelect(null, "constr")
                .SkipTake(0, -1);
        }
    }
}
