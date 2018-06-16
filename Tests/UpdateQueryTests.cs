using System;
using System.Data;
using DapperMan.Core;
using DapperMan.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests.MockData;

namespace Tests
{
    [TestClass]
    public class UpdateQueryTests
    {
        [TestMethod]
        public void UpdateQuery_GenerateStatement_NoIdentity()
        {
            string expected = @"
UPDATE TableName
SET [Id] = @Id, [Field1] = @Field1
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Update("TableName", "constr")
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableNoIdentity>(null));
        }

        [TestMethod]
        public void UpdateQuery_GenerateStatement_WithIdentity_IncludesScopeIdentity()
        {
            string expected = @"
UPDATE TableName
SET [Field1] = @Field1
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Update("TableName", "constr")
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableWithIdentity>(null));
        }

        [TestMethod]
        public void UpdateQuery_GenerateStatement_WithUpdateIgnore()
        {
            string expected = @"
UPDATE TableName
SET [Field1] = @Field1
WHERE A = @a
AND B = @b;";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Update("TableName", "constr")
                .Where("A = @a")
                .Where("B = @b");

            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableWithUpdateIgnore>(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            ((IQueryGenerator)DapperQuery.Insert(null, "constr"))
                .GenerateStatement();
        }
    }
}
