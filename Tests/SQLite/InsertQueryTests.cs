using System;
using System.Data;
using DapperMan.Core;
using DapperMan.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests.MockData;

namespace Tests.SQLite
{
    [TestClass]
    public class InsertQueryTests
    {
        [TestMethod]
        public void InsertQuery_GenerateStatement_NoIdentity()
        {
            var conn = new Mock<IDbConnection>().Object;

            string expected = @"
INSERT INTO TableName
([Id], [Field1])
VALUES (@Id, @Field1);";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Insert("TableName", conn);
            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableNoIdentity>(null));
        }

        [TestMethod]
        public void InsertQuery_GenerateStatement_WithIdentity_IncludesScopeIdentity()
        {
            var conn = new Mock<IDbConnection>().Object;

            string expected = @"
INSERT INTO TableName
([Field1])
VALUES (@Field1);";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim()
                + "SELECT CAST(last_insert_rowid() as INT);";

            var query = DapperQuery.Insert("TableName", conn);
            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableWithIdentity>(null));
        }

        [TestMethod]
        public void InsertQuery_GenerateStatement_WithInsertIgnore()
        {
            var conn = new Mock<IDbConnection>().Object;

            string expected = @"
INSERT INTO TableName
([Field1])
VALUES (@Field1);";

            expected = expected
                .Replace(Environment.NewLine, " ")
                .Trim();

            var query = DapperQuery.Insert("TableName", conn);
            Assert.AreEqual(expected, ((IGenericQueryGenerator)query).GenerateStatement<TableWithInsertIgnore>(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InsertQuery_GenerateStatement_ThrowsWhenSourceNull()
        {
            var conn = new Mock<IDbConnection>().Object;

            ((IQueryGenerator)DapperQuery.Insert(null, conn))
                .GenerateStatement();
        }

        [TestMethod]
        public void InsertQuery_Execute_NonQueryWithNoIdentity()
        {
            var conn = new Mock<IDbConnection>().Object;
            var mock = new Mock<InsertQuery>(new object[] { "tablename", conn });

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
            var conn = new Mock<IDbConnection>().Object;
            var mock = new Mock<InsertQuery>(new object[] { "tablename", conn });

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
