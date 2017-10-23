using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Paradigm.ORM.Data.DatabaseAccess;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Mappers;
using Paradigm.ORM.Tests.Fixtures;
using Paradigm.ORM.Tests.Fixtures.MySql;
using Paradigm.ORM.Tests.Fixtures.PostgreSql;
using Paradigm.ORM.Tests.Fixtures.Sql;
using NUnit.Framework;
using Paradigm.ORM.Tests.Fixtures.Cql;

namespace Paradigm.ORM.Tests.Tests.Mappers
{
    [TestFixture]
    public class DatabaseReaderMapperAsyncTest
    {

        [TestCase(typeof(MySqlReaderMapperFixture), typeof(Mocks.MySql.AllColumnsClass))]
        [TestCase(typeof(SqlReaderMapperFixture), typeof(Mocks.Sql.AllColumnsClass))]
        [TestCase(typeof(PostgreSqlReaderMapperFixture), typeof(Mocks.PostgreSql.AllColumnsClass))]
        [TestCase(typeof(CqlReaderMapperFixture), typeof(Mocks.Cql.AllColumnsClass))]
        public async System.Threading.Tasks.Task ShouldMapPropertiesCorrectlyAsync(Type fixtureType, Type mappedType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as ReaderMapperFixtureBase;

            fixture.Should().NotBeNull();
            fixture.Invoking(x => x.CreateDatabase()).ShouldNotThrow();
            fixture.CreateTable();

            var entity = fixture.CreateNewEntity();

            using (var databaseAccess = new DatabaseAccess(fixture.Connector, mappedType))
            {
                databaseAccess.Insert(entity);
            }

            await fixture.Connector.ExecuteReaderAsync(fixture.SelectStatement, reader =>
             {
                 // Test mapping here
                 var results = new DatabaseReaderMapper(fixture.Connector, mappedType).Map(reader);

                 results.Count.Should().BeGreaterThan(0);
                 var retrievedEntity = results.First();
                 var properties = mappedType.GetTypeInfo().DeclaredProperties;

                 // Compare properties by reflection
                 foreach (var property in properties)
                 {
                     var expectedValue = property.GetValue(entity);
                     var retrievedValue = property.GetValue(retrievedEntity);

                     if (expectedValue is IEnumerable expectedCollection &&
                         retrievedValue is IEnumerable retrievedCollection)
                     {
                         var collection1 = expectedCollection.Cast<object>().ToList();
                         var collection2 = retrievedCollection.Cast<object>().ToList();

                         collection1.Count.Should().Be(collection2.Count);

                         for (var i = 0; i < collection1.Count; i++)
                             collection1[i].Equals(collection2[i]).Should().BeTrue();
                     }
                     else
                         expectedValue.Equals(retrievedValue).Should().BeTrue($"{property.Name} is not equal. Expected {expectedValue} != {retrievedValue}");
                 }
             });
        }

        [TestCase(typeof(MySqlReaderMapperFixture), typeof(Mocks.MySql.AllColumnsClass))]
        [TestCase(typeof(SqlReaderMapperFixture), typeof(Mocks.Sql.AllColumnsClass))]
        [TestCase(typeof(PostgreSqlReaderMapperFixture), typeof(Mocks.PostgreSql.AllColumnsClass))]
        [TestCase(typeof(CqlReaderMapperFixture), typeof(Mocks.Cql.AllColumnsClass))]
        public async System.Threading.Tasks.Task ShouldRetrieveTwoEntitiesAsync(Type fixtureType, Type mappedType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as ReaderMapperFixtureBase;

            fixture.Should().NotBeNull();
            fixture.Invoking(x => x.CreateDatabase()).ShouldNotThrow();
            fixture.CreateTable();

            using (var databaseAccess = new DatabaseAccess(fixture.Connector, mappedType))
            {
                databaseAccess.Insert(new object[] { fixture.CreateNewEntity(), fixture.CreateNewEntity() });
            }

            await fixture.Connector.ExecuteReaderAsync(fixture.SelectStatement, reader =>
             {
                 var results = new DatabaseReaderMapper(fixture.Connector, mappedType).Map(reader);
                 results.Should().HaveCount(2);
             });
        }

        [TestCase(typeof(MySqlReaderMapperFixture), typeof(Mocks.MySql.AllColumnsClass))]
        [TestCase(typeof(SqlReaderMapperFixture), typeof(Mocks.Sql.AllColumnsClass))]
        [TestCase(typeof(PostgreSqlReaderMapperFixture), typeof(Mocks.PostgreSql.AllColumnsClass))]
        [TestCase(typeof(CqlReaderMapperFixture), typeof(Mocks.Cql.AllColumnsClass))]
        public async System.Threading.Tasks.Task ShouldRetrieveZeroEntitiesAsync(Type fixtureType, Type mappedType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as ReaderMapperFixtureBase;

            fixture.Should().NotBeNull();
            fixture.Invoking(x => x.CreateDatabase()).ShouldNotThrow();
            fixture.CreateTable();

            await fixture.Connector.ExecuteReaderAsync(fixture.SelectStatement, reader =>
             {
                 var results = new DatabaseReaderMapper(fixture.Connector, mappedType).Map(reader);
                 results.Should().HaveCount(0);
             });
        }

        [TestCase(typeof(MySqlReaderMapperFixture), typeof(Mocks.MySql.AllColumnsClass))]
        [TestCase(typeof(SqlReaderMapperFixture), typeof(Mocks.Sql.AllColumnsClass))]
        [TestCase(typeof(PostgreSqlReaderMapperFixture), typeof(Mocks.PostgreSql.AllColumnsClass))]
        [TestCase(typeof(CqlReaderMapperFixture), typeof(Mocks.Cql.AllColumnsClass))]
        public async System.Threading.Tasks.Task ShouldThrowArgumentNullExceptionAsync(Type fixtureType, Type mappedType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as ReaderMapperFixtureBase;

            fixture.Should().NotBeNull();
            fixture.Invoking(x => x.CreateDatabase()).ShouldNotThrow();
            fixture.CreateTable();

            await fixture.Connector.ExecuteReaderAsync(fixture.SelectStatement, reader =>
             {
                 Action map = () => new DatabaseReaderMapper(fixture.Connector, mappedType).Map(null);
                 map.ShouldThrow<ArgumentNullException>();
             });
        }

        #region TearDown

        [TearDown]
        public void TearDown()
        {
            // We cleanup after each test to mantain consistency
            using (var fixtureSql = Activator.CreateInstance(typeof(SqlReaderMapperFixture)) as ReaderMapperFixtureBase)
            {
                fixtureSql.CreateDatabase();
                fixtureSql.DropDatabase();
            }

            using (var fixtureMySql = Activator.CreateInstance(typeof(MySqlReaderMapperFixture)) as ReaderMapperFixtureBase)
            {
                fixtureMySql.CreateDatabase();
                fixtureMySql.DropDatabase();
            }

            using (var fixturePostgreSql = Activator.CreateInstance(typeof(PostgreSqlReaderMapperFixture)) as ReaderMapperFixtureBase)
            {
                fixturePostgreSql.CreateDatabase();
                fixturePostgreSql.DropDatabase();
            }

            using (var fixtureCql = Activator.CreateInstance(typeof(CqlReaderMapperFixture)) as ReaderMapperFixtureBase)
            {
                fixtureCql.CreateDatabase();
                fixtureCql.DropDatabase();
            }
        }

        #endregion
    }

}
