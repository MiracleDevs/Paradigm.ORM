using System;
using Paradigm.ORM.Data.DatabaseAccess;
using Paradigm.ORM.Data.StoredProcedures;
using Paradigm.ORM.Tests.Fixtures.PostgreSql;
using Paradigm.ORM.Tests.Mocks.PostgreSql;
using Paradigm.ORM.Tests.Mocks.PostgreSql.Routines;
using NUnit.Framework;
using FluentAssertions;

namespace Paradigm.ORM.Tests.Tests.StoredProcedures.PostgreSql
{
    [TestFixture]
    public class PostgreSqlStoredProceduresTest
    {
        private PostgreSqlStoredProcedureFixture Fixture { get; }

        public PostgreSqlStoredProceduresTest()
        {
            Fixture = new PostgreSqlStoredProcedureFixture();
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Fixture.CreateDatabase();
            Fixture.CreateParentTable();
            Fixture.CreateChildTable();
            Fixture.CreateStoredProcedures();

            var databaseAccess = new DatabaseAccess(Fixture.Connector, typeof(SingleKeyParentTable));

            databaseAccess.Insert(Fixture.CreateNewActiveEntity());
            databaseAccess.Insert(Fixture.CreateNewActiveEntity());
            databaseAccess.Insert(Fixture.CreateNewInactiveEntity());
            databaseAccess.Insert(Fixture.CreateNewInactiveEntity());
        }

        [Test]
        public void ShouldRetrieveScalarResultOnActiveClients()
        {
            var args = new GetTotalAmountParameters
            {
                Active = true
            };
            var activeEntity = Fixture.CreateNewActiveEntity() as SingleKeyParentTable;

            var result = new ScalarStoredProcedure<GetTotalAmountParameters, decimal>(Fixture.Connector).ExecuteScalar(args);

            result.Should().Be(activeEntity.Amount * 2);
        }

        [Test]
        public void ShouldRetrieveScalarResultOnInactiveClients()
        {
            var args = new GetTotalAmountParameters
            {
                Active = false
            };
            var notActiveEntity = Fixture.CreateNewInactiveEntity() as SingleKeyParentTable;

            var result = new ScalarStoredProcedure<GetTotalAmountParameters, decimal>(Fixture.Connector).ExecuteScalar(args);

            result.Should().Be(notActiveEntity.Amount * 2);
        }

        /****************************************************************************************************************************
         * DUE TO BAD DRIVER DESIGN AND LACK OF AUTOMATIC CURSOR FETCH, WE HAVE TO DEPRECATE
         * MULTIPLE RESULTSETS FOR POSTGRE, AT LEAST UNTIL THE PEOPLE BEHIND THE DRIVER
         * ALLOW TO GO BACK TO PREVIOUS DEREFERENCING. SEE: https://github.com/npgsql/npgsql/issues/438
         * ***************************************************************************************************************************
        [Test]
        public void ShouldRetrieveMultipleResults()
        {
            var args = new SearchParentsAndChildsParameters
            {
                ParentName = "Test",
                Active = true
            };

            using (var transaction = this.Fixture.Connector.CreateTransaction())
            {
                var procedure = new ReaderStoredProcedure<SearchParentsAndChildsParameters, SingleKeyParentTable, SingleKeyChildTable>(Fixture.Connector);
                var results = procedure.ExecuteAsync(args).Result;

                results.Item1.Should().NotBeNull();
                results.Item1.Should().HaveCount(2);
                results.Item2.Should().NotBeNull();
                results.Item2.Should().HaveCount(4);

                var activeParentEntity = Fixture.CreateNewActiveEntity();
                foreach (var entity in results.Item1)
                {
                    entity.Name.Should().StartWith("Test Parent");
                    entity.ShouldBeEquivalentTo(activeParentEntity, options => options.Excluding(o => o.Name)
                                                                                      .Excluding(o => o.Id)
                                                                                      .Excluding(o => o.Childs));
                }

                var activeChildEntity = Fixture.CreateActiveChildEntity();
                foreach (var entity in results.Item2)
                {
                    entity.Name.Should().StartWith("Test Child");
                    entity.ShouldBeEquivalentTo(activeChildEntity, options => options.Excluding(o => o.Name)
                                                                                     .Excluding(o => o.Id)
                                                                                     .Excluding(o => o.ParentId));
                }

                transaction.Commit();
            }
        }
        */
        [Test]
        public void ShouldRetrieveTwoResultsOnActiveClients()
        {
            var args = new SearchParentTableParameters
            {
                ParentName = "Test Par",
                Active = true
            };

            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(Fixture.Connector).Execute(args);

            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            var activeParentEntity = Fixture.CreateNewActiveEntity();
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

            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(Fixture.Connector).Execute(args);

            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            var activeParentEntity = Fixture.CreateNewInactiveEntity();
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

            var nonQuery = new NonQueryStoredProcedure<UpdateRoutineParameters>(Fixture.Connector);
            nonQuery.ExecuteNonQuery(updateRoutineArgs);

            var searchClientArgs = new SearchParentTableParameters
            {
                ParentName = "Test Parent ChangedNameTest",
                Active = true
            };
            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(Fixture.Connector).Execute(searchClientArgs);

            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            results[0].Name.Should().Be("Test Parent ChangedNameTest");
        }

        [Test]
        public void ShouldThrowArgumentException()
        {
            Action results = () => new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(Fixture.Connector).Execute(null);
            results.ShouldThrow<ArgumentNullException>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Fixture.DeleteStoredProcedures();
            Fixture.DropDatabase();
            Fixture.Dispose();
        }
    }
}
