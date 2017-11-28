using System;
using FluentAssertions;
using NUnit.Framework;
using Paradigm.ORM.Data.DatabaseAccess;
using Paradigm.ORM.Data.StoredProcedures;
using Paradigm.ORM.Tests.Fixtures.Sql;
using Paradigm.ORM.Tests.Mocks.Sql;
using Paradigm.ORM.Tests.Mocks.Sql.Routines;

namespace Paradigm.ORM.Tests.Tests.StoredProcedures.Sql
{
    [TestFixture]
    public class SqlStoredProceduresTest
    {
        private SqlStoredProcedureFixture Fixture { get; }

        public SqlStoredProceduresTest()
        {
            this.Fixture = new SqlStoredProcedureFixture();
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this.Fixture.CreateDatabase();
            this.Fixture.CreateParentTable();
            this.Fixture.CreateChildTable();
            this.Fixture.CreateStoredProcedures();

            var databaseAccess = new DatabaseAccess(this.Fixture.Connector, typeof(SingleKeyParentTable));

            databaseAccess.Insert(this.Fixture.CreateNewActiveEntity());
            databaseAccess.Insert(this.Fixture.CreateNewActiveEntity());
            databaseAccess.Insert(this.Fixture.CreateNewInactiveEntity());
            databaseAccess.Insert(this.Fixture.CreateNewInactiveEntity());
        }

        [Test]
        public void ShouldRetrieveScalarResultOnActiveClients()
        {
            var args = new GetTotalAmountParameters
            {
                Active = true
            };
            var activeEntity = this.Fixture.CreateNewActiveEntity() as SingleKeyParentTable;

            var result = new ScalarStoredProcedure<GetTotalAmountParameters, decimal>(this.Fixture.Connector).ExecuteScalar(args);

            result.Should().Be(activeEntity.Amount * 2);
        }

        [Test]
        public void ShouldRetrieveScalarResultOnInactiveClients()
        {
            var args = new GetTotalAmountParameters
            {
                Active = false
            };
            var notActiveEntity = this.Fixture.CreateNewInactiveEntity() as SingleKeyParentTable;

            var result = new ScalarStoredProcedure<GetTotalAmountParameters, decimal>(this.Fixture.Connector).ExecuteScalar(args);

            result.Should().Be(notActiveEntity.Amount * 2);
        }

        [Test]
        public void ShouldRetrieveMultipleResults()
        {
            var args = new SearchParentsAndChildsParameters
            {
                ParentName = "Test",
                Active = true
            };

            var results = new ReaderStoredProcedure<SearchParentsAndChildsParameters, SingleKeyParentTable, SingleKeyChildTable>(this.Fixture.Connector).Execute(args);

            results.Item1.Should().NotBeNull();
            results.Item1.Should().HaveCount(2);
            results.Item2.Should().NotBeNull();
            results.Item2.Should().HaveCount(4);

            var activeParentEntity = this.Fixture.CreateNewActiveEntity();
            foreach (var entity in results.Item1)
            {
                entity.Name.Should().StartWith("Test Parent");
                entity.ShouldBeEquivalentTo(activeParentEntity, options => options.Excluding(o => o.Name)
                                                                                  .Excluding(o => o.Id)
                                                                                  .Excluding(o => o.Childs));
            }

            var activeChildEntity = this.Fixture.CreateActiveChildEntity();
            foreach (var entity in results.Item2)
            {
                entity.Name.Should().StartWith("Test Child");
                entity.ShouldBeEquivalentTo(activeChildEntity, options => options.Excluding(o => o.Name)
                                                                                 .Excluding(o => o.Id)
                                                                                 .Excluding(o => o.ParentId));
            }
        }

        [Test]
        public void ShouldRetrieveTwoResultsOnActiveClients()
        {
            var args = new SearchParentTableParameters
            {
                ParentName = "Test Par",
                Active = true
            };

            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(this.Fixture.Connector).Execute(args);

            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            var activeParentEntity = this.Fixture.CreateNewActiveEntity();
            foreach (var entity in results)
            {
                entity.Name.Should().StartWith("Test Parent");
                entity.ShouldBeEquivalentTo(activeParentEntity, options => options.Excluding(o => o.Name)
                                                                                  .Excluding(o => o.Id)
                                                                                  .Excluding(o => o.Childs));
            }
        }

        [Test]
        public void ShouldRetrieveTwoResultsOnInactiveClients()
        {
            var args = new SearchParentTableParameters
            {
                ParentName = "Test Par",
                Active = false
            };

            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(this.Fixture.Connector).Execute(args);

            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            var activeParentEntity = this.Fixture.CreateNewInactiveEntity();
            foreach (var entity in results)
            {
                entity.Name.Should().StartWith("Test Parent");
                entity.ShouldBeEquivalentTo(activeParentEntity, options => options.Excluding(o => o.Name)
                                                                                  .Excluding(o => o.Id)
                                                                                  .Excluding(o => o.Childs));
            }
        }

        [Test]
        public void NonQueryProcedureShouldModifyEntity()
        {
            var updateRoutineArgs = new UpdateRoutineParameters
            {
                Id = 1,
            };

            var nonQuery = new NonQueryStoredProcedure<UpdateRoutineParameters>(this.Fixture.Connector);
            nonQuery.ExecuteNonQuery(updateRoutineArgs);

            var searchClientArgs = new SearchParentTableParameters
            {
                ParentName = "Test Parent ChangedNameTest",
                Active = true
            };
            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(this.Fixture.Connector).Execute(searchClientArgs);

            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            results[0].Name.Should().Be("Test Parent ChangedNameTest");
        }

        [Test]
        public void ShouldThrowArgumentException()
        {
            Action results = () => new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(this.Fixture.Connector).Execute(null);
            results.ShouldThrow<ArgumentNullException>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.Fixture.DeleteStoredProcedures();
            this.Fixture.DropDatabase();
            this.Fixture.Dispose();
        }
    }
}
