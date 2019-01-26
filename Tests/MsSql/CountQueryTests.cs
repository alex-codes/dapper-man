using System;
using DapperMan.Core;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.MsSql
{
    [TestClass]
    public class CountQueryTests
    {
        [TestMethod]
        public void CountQuery_GenerateStatement()
        {
            string expected = @"
SELECT [Count] = COUNT(*)
FROM dbo.TableName
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Count("dbo.TableName", "constr")
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            ((IQueryGenerator)DapperQuery.Count(null, "constr"))
                .GenerateStatement();
        }
    }
}
