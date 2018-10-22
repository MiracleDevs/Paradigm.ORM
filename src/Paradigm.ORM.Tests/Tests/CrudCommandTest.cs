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
    public partial class CrudCommandTest
    {
        [Order(1)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldCreateDatabaseAndDropIt(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Should().NotBeNull();
            fixture.Invoking(x => x.CreateDatabase()).Should().NotThrow();
            fixture.Invoking(x => x.DropDatabase()).Should().NotThrow();
        }

        [Order(2)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldCreateDatabaseAndTables(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Invoking(x => x.CreateDatabase()).Should().NotThrow();
            fixture.Invoking(x => x.CreateTables()).Should().NotThrow();
            fixture.Invoking(x => x.DropDatabase()).Should().NotThrow();
        }

        [Order(3)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldInsertData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());

            var entityToInsert = fixture.CreateNewEntity();

            entityToInsert.Should().NotBeNull();
            insertCommandBuilder.Should().NotBeNull();

            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { entityToInsert });
            valueProvider.MoveNext();

            var insertCommand = insertCommandBuilder.GetCommand(valueProvider);

            if (fixtureType == typeof(CqlCrudCommandFixture))
                insertCommand.ExecuteNonQuery().Should().Be(-1);
            else
                insertCommand.ExecuteNonQuery().Should().Be(1);

            fixture.DropDatabase();
        }

        [Order(4)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        public void ShouldGetTheLastInsertedIdData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Should().NotBeNull();
            fixture.CreateDatabase();
            fixture.CreateTables();

            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { fixture.CreateNewEntity() });
            valueProvider.MoveNext();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());
            var lastInsertIdCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateLastInsertIdCommandBuilder(fixture.GetParentDescriptor());

            var insertCommand = insertCommandBuilder.GetCommand(valueProvider);
            var lastInsertCommand = lastInsertIdCommandBuilder.GetCommand();
            insertCommand.CommandText += fixture.Connector.GetCommandFormatProvider().GetQuerySeparator() + lastInsertCommand.CommandText;

            using (var reader = insertCommand.ExecuteReader())
            {
                reader.Should().NotBeNull();
                reader.Read().Should().BeTrue();
                Convert.ToInt32(reader.GetValue(0)).Should().BeGreaterThan(0);
            }


            fixture.DropDatabase();
        }

        [Order(5)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldSelectAllData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Should().NotBeNull();
            fixture.CreateDatabase();
            fixture.CreateTables();

            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { fixture.CreateNewEntity(), fixture.CreateNewEntity() });

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());

            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

            var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor());
            selectCommandBuilder.Should().NotBeNull();
            var selectCommand = selectCommandBuilder.GetCommand();

            using (var reader = selectCommand.ExecuteReader())
            {
                reader.Should().NotBeNull();

                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                var results = mapper.Map(reader);

                results.Should().NotBeNull().And.HaveCount(2);
            }

            fixture.DropDatabase();
        }

        [Order(6)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldSelectOneData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Should().NotBeNull();
            fixture.CreateDatabase();
            fixture.CreateTables();

            var first = fixture.CreateNewEntity();
            var second = fixture.CreateNewEntity();

            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());

            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

            var selectOneCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectOneCommandBuilder(fixture.GetParentDescriptor());

            selectOneCommandBuilder.Should().NotBeNull();
            var selectCommand = selectOneCommandBuilder.GetCommand(1);

            using (var reader = selectCommand.ExecuteReader())
            {
                reader.Should().NotBeNull();

                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                var results = mapper.Map(reader);
                results.Should().NotBeNull().And.HaveCount(1);
            }

            fixture.DropDatabase();
        }

        [Order(7)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldDeleteData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Should().NotBeNull();
            fixture.CreateDatabase();
            fixture.CreateTables();

            var first = fixture.CreateNewEntity();
            var second = fixture.CreateNewEntity();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());
            {
                var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            }

            var deleteCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateDeleteCommandBuilder(fixture.GetParentDescriptor());
            {
                fixture.SetEntityId(first, second);
                var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

                valueProvider.MoveNext();
                var deleteCommand = deleteCommandBuilder.GetCommand(valueProvider);
                deleteCommand.Invoking(c => c.ExecuteNonQuery()).Should().NotThrow();

                valueProvider.MoveNext();
                deleteCommand = deleteCommandBuilder.GetCommand(valueProvider);
                deleteCommand.Invoking(c => c.ExecuteNonQuery()).Should().NotThrow();
            }

            var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor());
            {
                var selectCommand = selectCommandBuilder.GetCommand();

                using (var reader = selectCommand.ExecuteReader())
                {
                    var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                    var results = mapper.Map(reader);
                    results.Should().NotBeNull().And.HaveCount(0);
                }
            }

            fixture.DropDatabase();
        }

        [Order(8)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldUpdateData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Should().NotBeNull();
            fixture.CreateDatabase();
            fixture.CreateTables();

            var first = fixture.CreateNewEntity();
            var second = fixture.CreateNewEntity();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor());
            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

            var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor());
            var selectCommand = selectCommandBuilder.GetCommand();

            using (var reader = selectCommand.ExecuteReader())
            {
                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                var results = mapper.Map(reader);

                first = results[0];
                second = results[1];
            }

            var updateCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateUpdateCommandBuilder(fixture.GetParentDescriptor());

            fixture.Update(first, second);
            valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

            updateCommandBuilder.Should().NotBeNull();

            valueProvider.MoveNext();
            var updateCommand = updateCommandBuilder.GetCommand(valueProvider);
            updateCommand.Invoking(c => c.ExecuteNonQuery()).Should().NotThrow();

            valueProvider.MoveNext();
            updateCommand = updateCommandBuilder.GetCommand(valueProvider);
            updateCommand.Invoking(c => c.ExecuteNonQuery()).Should().NotThrow();

            selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor());
            selectCommand = selectCommandBuilder.GetCommand();

            using (var reader = selectCommand.ExecuteReader())
            {
                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                var results = mapper.Map(reader);

                first = results[0];
                second = results[1];

                fixture.CheckUpdate(first, second);
            }

            fixture.DropDatabase();
        }

        [Order(9)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldInsertMultipleKeyData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetMultipleKeyDescriptor());

            var entityToInsert = fixture.CreateNewTwoKeysEntity();
            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { entityToInsert });
            valueProvider.MoveNext();

            var insertCommand = insertCommandBuilder.GetCommand(valueProvider);
            insertCommand.ExecuteNonQuery();

            fixture.DropDatabase();
        }

        [Order(10)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldSelectAllMultipleKeyData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { fixture.CreateNewTwoKeysEntity(), fixture.CreateNewTwoKeysEntity() });

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetMultipleKeyDescriptor());

            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

            var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetMultipleKeyDescriptor());
            var selectCommand = selectCommandBuilder.GetCommand();

            using (var reader = selectCommand.ExecuteReader())
            {
                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetMultipleKeyDescriptor());
                var results = mapper.Map(reader);

                results.Should().NotBeNull().And.HaveCount(2);
            }

            fixture.DropDatabase();
        }

        [Order(11)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldSelectOneMultipleKeyData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var first = fixture.CreateNewTwoKeysEntity();
            var second = fixture.CreateNewTwoKeysEntity();

            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetMultipleKeyDescriptor());

            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

            var selectOneCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectOneCommandBuilder(fixture.GetMultipleKeyDescriptor());
            var selectCommand = selectOneCommandBuilder.GetCommand(1, 2);

            using (var reader = selectCommand.ExecuteReader())
            {
                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetMultipleKeyDescriptor());
                var results = mapper.Map(reader);
                results.Should().NotBeNull().And.HaveCount(1);
            }

            fixture.DropDatabase();
        }

        [Order(12)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldDeleteMultipleKeyData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var first = fixture.CreateNewTwoKeysEntity();
            var second = fixture.CreateNewTwoKeysEntity();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetMultipleKeyDescriptor());
            {
                var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            }

            var deleteCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateDeleteCommandBuilder(fixture.GetMultipleKeyDescriptor());
            {
                var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

                valueProvider.MoveNext();
                var deleteCommand = deleteCommandBuilder.GetCommand(valueProvider);
                deleteCommand.Invoking(c => c.ExecuteNonQuery()).Should().NotThrow();

                valueProvider.MoveNext();
                deleteCommand = deleteCommandBuilder.GetCommand(valueProvider);
                deleteCommand.Invoking(c => c.ExecuteNonQuery()).Should().NotThrow();
            }

            var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetMultipleKeyDescriptor());
            {
                var selectCommand = selectCommandBuilder.GetCommand();

                using (var reader = selectCommand.ExecuteReader())
                {
                    var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetMultipleKeyDescriptor());
                    var results = mapper.Map(reader);
                    results.Should().NotBeNull().And.HaveCount(0);
                }
            }

            fixture.DropDatabase();
        }

        [Order(13)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldUpdateMultipleKeyData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.CreateDatabase();
            fixture.CreateTables();

            var first = fixture.CreateNewTwoKeysEntity();
            var second = fixture.CreateNewTwoKeysEntity();

            var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetMultipleKeyDescriptor());
            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

            valueProvider.MoveNext();
            insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

            var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetMultipleKeyDescriptor());
            var selectCommand = selectCommandBuilder.GetCommand();

            using (var reader = selectCommand.ExecuteReader())
            {
                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetMultipleKeyDescriptor());
                var results = mapper.Map(reader);

                first = results[0];
                second = results[1];
            }

            var updateCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateUpdateCommandBuilder(fixture.GetMultipleKeyDescriptor());

            valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { first, second });

            valueProvider.MoveNext();
            var updateCommand = updateCommandBuilder.GetCommand(valueProvider);
            updateCommand.Invoking(c => c.ExecuteNonQuery()).Should().NotThrow();

            valueProvider.MoveNext();
            updateCommand = updateCommandBuilder.GetCommand(valueProvider);
            updateCommand.Invoking(c => c.ExecuteNonQuery()).Should().NotThrow();

            selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetMultipleKeyDescriptor());
            selectCommand = selectCommandBuilder.GetCommand();

            using (var reader = selectCommand.ExecuteReader())
            {
                var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetMultipleKeyDescriptor());
                var results = mapper.Map(reader);

                results.Should().NotBeNullOrEmpty();
            }

            fixture.DropDatabase();
        }
    }
}
