using System;
using System.Collections.Generic;
using FluentAssertions;
using Paradigm.ORM.Data.ValueProviders;
using Paradigm.ORM.Tests.Fixtures;
using Paradigm.ORM.Tests.Fixtures.MySql;
using Paradigm.ORM.Tests.Fixtures.PostgreSql;
using Paradigm.ORM.Tests.Fixtures.Sql;
using Paradigm.ORM.Tests.Fixtures.Cql;
using NUnit.Framework;
using Paradigm.ORM.Data.Mappers;


namespace Paradigm.ORM.Tests.Tests
{
    [TestFixture]
    public class TransactionTest
    {
        [Order(1)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        public void ShouldCreateATransaction(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());
            var entityToInsert = fixture.CreateNewEntity();
            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { entityToInsert });
            valueProvider.MoveNext();
            var insertCommand = insertCommandBuilder.GetCommand(valueProvider);

            using (var transaction = fixture.Connector.CreateTransaction())
            {
                if (fixtureType == typeof(CqlCrudCommandFixture))
                    insertCommand.ExecuteNonQuery().Should().Be(-1);
                else
                    insertCommand.ExecuteNonQuery().Should().Be(1);
            }

            fixture.DropDatabase();
        }

        [Order(2)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        public void ShouldCommitATransaction(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());
            var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor());
            var entityToInsert = fixture.CreateNewEntity();
            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { entityToInsert });
            valueProvider.MoveNext();
            var insertCommand = insertCommandBuilder.GetCommand(valueProvider);
            var selectCommand = selectCommandBuilder.GetCommand();

            using (var transaction = fixture.Connector.CreateTransaction())
            {
                if (fixtureType == typeof(CqlCrudCommandFixture))
                    insertCommand.ExecuteNonQuery().Should().Be(-1);
                else
                    insertCommand.ExecuteNonQuery().Should().Be(1);

                transaction.Commit();
            }

            using (var reader = selectCommand.ExecuteReader())
            {
                reader.Should().NotBeNull();

                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                var results = mapper.Map(reader);

                results.Should().NotBeNull().And.HaveCount(1);
            }

            fixture.DropDatabase();
        }

        [Order(3)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        public void ShouldRollbackATransaction(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());
            var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor());
            var entityToInsert = fixture.CreateNewEntity();
            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { entityToInsert });
            valueProvider.MoveNext();
            var insertCommand = insertCommandBuilder.GetCommand(valueProvider);
            var selectCommand = selectCommandBuilder.GetCommand();

            using (var transaction = fixture.Connector.CreateTransaction())
            {
                if (fixtureType == typeof(CqlCrudCommandFixture))
                    insertCommand.ExecuteNonQuery().Should().Be(-1);
                else
                    insertCommand.ExecuteNonQuery().Should().Be(1);

                transaction.Rollback();
            }

            using (var reader = selectCommand.ExecuteReader())
            {
                reader.Should().NotBeNull();

                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                var results = mapper.Map(reader);

                results.Should().NotBeNull().And.HaveCount(0);
            }

            fixture.DropDatabase();
        }

    }
}
