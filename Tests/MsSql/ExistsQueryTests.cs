using System;
using DapperMan.Core;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.MsSql
{
    [TestClass]
    public class ExistsQueryTests
    {
        [TestMethod]
        public void ExistsQuery_GenerateStatement()
        {
            string expected = @"
SELECT TOP 1 *
FROM dbo.TableName
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Exists("dbo.TableName", "constr")
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExistsQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            ((IQueryGenerator)DapperQuery.Exists(null, "constr"))
                .GenerateStatement();
        }
    }
}
