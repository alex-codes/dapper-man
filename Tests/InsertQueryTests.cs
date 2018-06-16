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
    public class InsertQueryTests
    {
        [TestMethod]
        public void InsertQuery_GenerateStatement_NoIdentity()
        {
            string expected = @"
INSERT INTO TableName
([Id], [Field1])
VALUES (@Id, @Field1);";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Insert("TableName", "constr");
            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableNoIdentity>(null));
        }

        [TestMethod]
        public void InsertQuery_GenerateStatement_WithIdentity_IncludesScopeIdentity()
        {
            string expected = @"
INSERT INTO TableName
([Field1])
VALUES (@Field1);";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim()
                + "SELECT CAST(SCOPE_IDENTITY() as int);";

            var query = DapperQuery.Insert("TableName", "constr");
            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableWithIdentity>(null));
        }

        [TestMethod]
        public void InsertQuery_GenerateStatement_WithInsertIgnore()
        {
            string expected = @"
INSERT INTO TableName
([Field1])
VALUES (@Field1);";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Insert("TableName", "constr");
            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableWithInsertIgnore>(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InsertQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            ((IQueryGenerator)DapperQuery.Insert(null, "constr"))
                .GenerateStatement();
        }

        [TestMethod]
        public void InsertQuery_Execute_NonQueryWithNoIdentity()
        {
            var mock = new Mock<InsertQuery>(new object[] { "tablename", "constr" });

            mock.Setup(m => m.ExecuteNonQuery(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<CommandType>(),
                    It.IsAny<IDbTransaction>()))
                .Returns(0);

            mock.CallBase = true;
            mock.Object.Execute<TableNoIdentity>();

            mock.Verify(m => m.ExecuteNonQuery(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<CommandType>(),
                    It.IsAny<IDbTransaction>()), Times.Once);

            mock.Verify(m => m.Query<int>(
                   It.IsAny<string>(),
                   It.IsAny<object>(),
                   It.IsAny<CommandType>(),
                   It.IsAny<IDbTransaction>()), Times.Never);
        }

        [TestMethod]
        public void InsertQuery_Execute_QueryWithIdentity()
        {
            var mock = new Mock<InsertQuery>(new object[] { "tablename", "constr" });

            mock.Setup(m => m.Query<int>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<CommandType>(),
                    It.IsAny<IDbTransaction>()))
                .Returns(new[] { 0 });

            mock.CallBase = true;
            mock.Object.Execute<TableWithIdentity>();

            mock.Verify(m => m.ExecuteNonQuery(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<CommandType>(),
                    It.IsAny<IDbTransaction>()), Times.Never);

            mock.Verify(m => m.Query<int>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<CommandType>(),
                    It.IsAny<IDbTransaction>()), Times.Once);
        }
    }
}
