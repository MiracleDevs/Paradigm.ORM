using System;
using FluentAssertions;
using Paradigm.ORM.Data.DatabaseAccess;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Querying;
using Paradigm.ORM.Tests.Fixtures;
using Paradigm.ORM.Tests.Fixtures.MySql;
using Paradigm.ORM.Tests.Mocks.MySql;
using NUnit.Framework;
using Paradigm.ORM.Data.Exceptions;

namespace Paradigm.ORM.Tests.Tests.Queries.MySql
{
    [TestFixture]
    public class MySqlQueryTests
    {
        private QueryFixtureBase Fixture { get; }

        public MySqlQueryTests()
        {
            Fixture = Activator.CreateInstance(typeof(MySqlQueryFixture)) as QueryFixtureBase;
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Fixture.CreateDatabase();
            Fixture.CreateParentTable();
            Fixture.CreateChildTable();

            var databaseAccess = new DatabaseAccess(Fixture.Connector, typeof(SingleKeyParentTable));

            databaseAccess.Insert(Fixture.CreateNewEntity());
            databaseAccess.Insert(Fixture.CreateNewEntity2());
        }

        [Test]
        public void ShouldRetrieveTwoEntities()
        {
            var result = Fixture.Connector.Query<SingleKeyParentTable>();
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            var entity1 = result.Find(x => x.Name.StartsWith("Test Parent 1"));
            entity1.IsActive.Should().Be(true);
            entity1.Childs.Should().BeNull();
            entity1.CreatedDate.Should().Be(new DateTime(2017, 4, 12, 16, 25, 56, 0));
            entity1.Amount.Should().Be(30.34m);

            var entity2 = result.Find(x => x.Name.StartsWith("Test Parent 2"));
            entity2.IsActive.Should().Be(false);
            entity2.Childs.Should().BeNull();
            entity2.CreatedDate.Should().Be(new DateTime(2017, 6, 21, 13, 55, 46, 0));
            entity2.Amount.Should().Be(215.5m);
        }

        [Test]
        public void ShouldThrowMySqlException()
        {
            Action result = () => Fixture.Connector.Query<AllColumnsClass>();
            result.Should().Throw<DatabaseCommandException>().WithMessage(DatabaseCommandException.DefaultMessage).And.Command.Should().NotBeNull();
        }

        [Test]
        public void QueryWithWhere()
        {
            var result = Fixture.Connector.Query<SingleKeyParentTable>($"`{nameof(SingleKeyParentTable.Name)}` LIKE @1", "Test Parent 1%");
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].Name.Should().StartWith("Test Parent 1");
            result[0].IsActive.Should().Be(true);
            result[0].Childs.Should().BeNull();
            result[0].CreatedDate.Should().Be(new DateTime(2017, 4, 12, 16, 25, 56, 0));
            result[0].Amount.Should().Be(30.34m);
        }

        [Test]
        public void QueryWithNotMatchingWhere()
        {
            var result = Fixture.Connector.Query<SingleKeyParentTable>($"`{nameof(SingleKeyParentTable.Name)}` LIKE @1", "Non Existing Entity%");
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Test]
        public void QueryObjectMustBeReutilizable()
        {
            var query = new Query<SingleKeyParentTable>(Fixture.Connector);

            var result = query.Execute();
            var result2 = query.Execute();

            result.Should().NotBeNull();
            result2.Should().NotBeNull();
            result2.Should().HaveSameCount(result);
        }

        [Test]
        public void WhereClauseShouldNotRemainInObject()
        {
            var query = new Query<SingleKeyParentTable>(Fixture.Connector);

            var result = query.Execute();
            var result2 = query.Execute($"`{nameof(SingleKeyParentTable.Name)}` LIKE @1", "Test Parent 1%");

            result.Should().NotBeNull();
            result2.Should().NotBeNull();
            result2.Count.Should().NotBe(result.Count);
        }

        [Test]
        public void WhereClauseShouldNotRemainInObject2()
        {
            var query = new Query<SingleKeyParentTable>(Fixture.Connector);

            var result = query.Execute($"`{nameof(SingleKeyParentTable.Name)}` LIKE @1", "Test Parent 1%");
            var result2 = query.Execute();

            result.Should().NotBeNull();
            result2.Should().NotBeNull();
            result2.Count.Should().NotBe(result.Count);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Fixture.DropDatabase();
            Fixture.Dispose();
        }
    }
}
