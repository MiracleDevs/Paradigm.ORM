﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Paradigm.ORM.Data.DatabaseAccess;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Querying;
using Paradigm.ORM.Tests.Fixtures;
using Paradigm.ORM.Tests.Fixtures.Cql;
using Paradigm.ORM.Tests.Mocks.Cql;

namespace Paradigm.ORM.Tests.Tests.Queries.Cql
{
    [TestFixture]
    public class CqlQueryAsyncTests
    {
        private QueryFixtureBase Fixture { get; }

        public CqlQueryAsyncTests()
        {
            this.Fixture = Activator.CreateInstance(typeof(CqlQueryFixture)) as QueryFixtureBase;
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this.Fixture.CreateDatabase();
            this.Fixture.CreateParentTable();
            this.Fixture.CreateChildTable();

            var databaseAccess = new DatabaseAccess(this.Fixture.Connector, typeof(SingleKeyParentTable));

            databaseAccess.Insert(this.Fixture.CreateNewEntity());
            databaseAccess.Insert(this.Fixture.CreateNewEntity2());
        }

        [Test]
        [Order(1)]
        public async Task ShouldRetrieveTwoEntitiesAsync()
        {
            var result = await this.Fixture.Connector.QueryAsync<SingleKeyParentTable>();
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            var entity1 = result.Find(x => x.Name.StartsWith("Test Parent 1"));
            entity1.IsActive.Should().Be(true);
            entity1.Childs.Should().BeNull();
            entity1.CreatedDate.Should().Be(new DateTime(2017, 4, 12, 0, 0, 0, 0));
            entity1.Amount.Should().Be(30.34m);

            var entity2 = result.Find(x => x.Name.StartsWith("Test Parent 2"));
            entity2.IsActive.Should().Be(false);
            entity2.Childs.Should().BeNull();
            entity2.CreatedDate.Should().Be(new DateTime(2017, 6, 21, 0, 0, 0, 0));
            entity2.Amount.Should().Be(215.5m);
        }

        [Test]
        [Order(2)]
        public async Task ShouldThrowCqlException()
        {
            Func<Task> result = async () => await this.Fixture.Connector.QueryAsync<AllColumnsClass>();
            (await result.Should().ThrowAsync<DatabaseCommandException>()).WithMessage(DatabaseCommandException.DefaultMessage).And.Command.Should().NotBeNull();
        }

        [Test]
        [Order(3)]
        public async Task QueryAsyncWithWhereAsync()
        {
            var result = await this.Fixture.Connector.QueryAsync<SingleKeyParentTable>($"\"{nameof(SingleKeyParentTable.Id)}\"=@1", 1);
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].Name.Should().StartWith("Test Parent 1");
            result[0].IsActive.Should().Be(true);
            result[0].Childs.Should().BeNull();
            result[0].CreatedDate.Should().Be(new DateTime(2017, 4, 12, 0, 0, 0, 0));
            result[0].Amount.Should().Be(30.34m);
        }

        [Test]
        [Order(4)]
        public async Task QueryAsyncWithNotMatchingWhereAsync()
        {
            var result = await this.Fixture.Connector.QueryAsync<SingleKeyParentTable>($"\"{nameof(SingleKeyParentTable.Id)}\"=@1", 10);
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Test]
        [Order(5)]
        public async Task QueryAsyncObjectMustBeReutilizableAsync()
        {
            var queryAsync = new Query<SingleKeyParentTable>(this.Fixture.Connector);

            var result = await queryAsync.ExecuteAsync();
            var result2 = await queryAsync.ExecuteAsync();

            result.Should().NotBeNull();
            result2.Should().NotBeNull();
            result2.Should().HaveSameCount(result);
        }

        [Test]
        [Order(6)]
        public async Task WhereClauseShouldNotRemainInObjectAsync()
        {
            var queryAsync = new Query<SingleKeyParentTable>(this.Fixture.Connector);

            var result = await queryAsync.ExecuteAsync();
            var result2 = await queryAsync.ExecuteAsync($"\"{nameof(SingleKeyParentTable.Id)}\"=@1", 1);

            result.Should().NotBeNull();
            result2.Should().NotBeNull();
            result2.Count.Should().NotBe(result.Count);
        }

        [Test]
        [Order(7)]
        public async Task WhereClauseShouldNotRemainInObject2Async()
        {
            var queryAsync = new Query<SingleKeyParentTable>(this.Fixture.Connector);

            var result = await queryAsync.ExecuteAsync($"\"{nameof(SingleKeyParentTable.Id)}\"=@1", 1);
            var result2 = await queryAsync.ExecuteAsync();

            result.Should().NotBeNull();
            result2.Should().NotBeNull();
            result2.Count.Should().NotBe(result.Count);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.Fixture.DropDatabase();
            this.Fixture.Dispose();
        }
    }
}
