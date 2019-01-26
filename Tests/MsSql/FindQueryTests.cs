using System;
using DapperMan.Core;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.MsSql
{
    [TestClass]
    public class FindQueryTests
    {
        [TestMethod]
        public void FindQuery_GenerateStatement()
        {
            string expected = @"
SELECT TOP 1 *
FROM dbo.TableName
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Find("dbo.TableName", "constr")
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            ((IQueryGenerator)DapperQuery.Find(null, "constr"))
                .GenerateStatement();
        }
    }
}
