using System;
using DapperMan.Core;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DeleteQueryTests
    {
        [TestMethod]
        public void DeleteQuery_GenerateStatement()
        {
            string expected = @"
DELETE FROM dbo.TableName
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Delete("dbo.TableName", "constr")
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IQueryGenerator)query).GenerateStatement());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            ((IQueryGenerator)DapperQuery.Delete(null, "constr"))
                .GenerateStatement();
        }
    }
}
