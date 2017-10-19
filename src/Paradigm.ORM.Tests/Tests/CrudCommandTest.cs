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
    public class CrudCommandTest
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
            fixture.Invoking(x => x.CreateDatabase()).ShouldNotThrow();
            fixture.Invoking(x => x.DropDatabase()).ShouldNotThrow();
        }

        [Order(2)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldCreateDatabaseAndTables(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Should().NotBeNull();
            fixture.Invoking(x => x.CreateDatabase()).ShouldNotThrow();
            fixture.Invoking(x => x.CreateParentTable()).ShouldNotThrow();
            fixture.Invoking(x => x.CreateChildTable()).ShouldNotThrow();
            fixture.Invoking(x => x.DropDatabase()).ShouldNotThrow();
        }

        [Order(3)]
        [TestCase(typeof(MySqlCrudCommandFixture))]
        [TestCase(typeof(SqlCrudCommandFixture))]
        [TestCase(typeof(PostgreSqlCrudCommandFixture))]
        [TestCase(typeof(CqlCrudCommandFixture))]
        public void ShouldInsertData(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CrudCommandFixtureBase;

            fixture.Should().NotBeNull();
            fixture.CreateDatabase();
            fixture.CreateParentTable();
            fixture.CreateChildTable();

            using (var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor()))
            {
                var entityToInsert = fixture.CreateNewEntity();

                entityToInsert.Should().NotBeNull();
                insertCommandBuilder.Should().NotBeNull();

                var valueProvider = new ClassValueProvider(new List<object> { entityToInsert });
                valueProvider.MoveNext();

                var insertCommand = insertCommandBuilder.GetCommand(valueProvider);

                insertCommand.Should().NotBeNull();
                insertCommand.CommandText.Should().Be(fixture.InsertParentStatement);

                if (fixtureType == typeof(CqlCrudCommandFixture))
                    insertCommand.ExecuteNonQuery().Should().Be(-1);
                else
                    insertCommand.ExecuteNonQuery().Should().Be(1);
            }

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
            fixture.CreateParentTable();
            fixture.CreateChildTable();

            var valueProvider = new ClassValueProvider(new List<object> { fixture.CreateNewEntity() });
            valueProvider.MoveNext();

            using (var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor()))
            {
                using (var lastInsertIdCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateLastInsertIdCommandBuilder())
                {
                    var insertCommand = insertCommandBuilder.GetCommand(valueProvider);

                    var lastInsertCommand = lastInsertIdCommandBuilder.GetCommand();
                    lastInsertCommand.Should().NotBeNull();
                    lastInsertCommand.CommandText.Should().Be(fixture.LastInsertedIdStatement);

                    insertCommand.CommandText += fixture.Connector.GetCommandFormatProvider().GetQuerySeparator() + lastInsertCommand.CommandText;

                    using (var reader = insertCommand.ExecuteReader())
                    {
                        reader.Should().NotBeNull();
                        reader.Read().Should().BeTrue();
                        Convert.ToInt32(reader.GetValue(0)).Should().BeGreaterThan(0);
                    }
                }
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
            fixture.CreateParentTable();
            fixture.CreateChildTable();

            var valueProvider = new ClassValueProvider(new List<object> { fixture.CreateNewEntity(), fixture.CreateNewEntity() });

            using (var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor()))
            {
                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            }

            using (var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor()))
            {
                selectCommandBuilder.Should().NotBeNull();
                var selectCommand = selectCommandBuilder.GetCommand();

                selectCommand.Should().NotBeNull();
                selectCommand.CommandText.Should().Be(fixture.SelectStatement);

                using (var reader = selectCommand.ExecuteReader())
                {
                    reader.Should().NotBeNull();

                    var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                    var results = mapper.Map(reader);

                    results.Should().NotBeNull().And.HaveCount(2);
                }
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
            fixture.CreateParentTable();
            fixture.CreateChildTable();

            var first = fixture.CreateNewEntity();
            var second = fixture.CreateNewEntity();

            var valueProvider = new ClassValueProvider(new List<object> { first, second });


            using (var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor()))
            {
                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            }

            using (var selectOneCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectOneCommandBuilder(fixture.GetParentDescriptor()))
            {
                selectOneCommandBuilder.Should().NotBeNull();
                var selectCommand = selectOneCommandBuilder.GetCommand(1);

                selectCommand.Should().NotBeNull();
                selectCommand.CommandText.Should().Be(fixture.SelectOneStatement);

                using (var reader = selectCommand.ExecuteReader())
                {
                    reader.Should().NotBeNull();

                    var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                    var results = mapper.Map(reader);
                    results.Should().NotBeNull().And.HaveCount(1);
                }
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
            fixture.CreateParentTable();
            fixture.CreateChildTable();

            var first = fixture.CreateNewEntity();
            var second = fixture.CreateNewEntity();
      
            using (var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor()))
            {
                var valueProvider = new ClassValueProvider(new List<object> { first, second });

                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            }

            using (var deleteCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateDeleteCommandBuilder(fixture.GetParentDescriptor()))
            {
                fixture.SetEntityId(first, second);
                var valueProvider = new ClassValueProvider(new List<object> { first, second });

                deleteCommandBuilder.Should().NotBeNull();
                var deleteCommand = deleteCommandBuilder.GetCommand(valueProvider);

                deleteCommand.Should().NotBeNull();
                deleteCommand.CommandText.Should().Be(fixture.DeleteStatement);
                deleteCommand.Invoking(c => c.ExecuteNonQuery()).ShouldNotThrow();
             }

            using (var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor()))
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
            fixture.CreateParentTable();
            fixture.CreateChildTable();

            var first = fixture.CreateNewEntity();
            var second = fixture.CreateNewEntity();

            using (var insertCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateInsertCommandBuilder(fixture.GetParentDescriptor()))
            {
                var valueProvider = new ClassValueProvider(new List<object> { first, second });

                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();

                valueProvider.MoveNext();
                insertCommandBuilder.GetCommand(valueProvider).ExecuteNonQuery();
            }

            using (var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor()))
            {
                var selectCommand = selectCommandBuilder.GetCommand();

                using (var reader = selectCommand.ExecuteReader())
                {
                    var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                    var results = mapper.Map(reader);

                    first = results[0];
                    second = results[1];
                }
            }

            using (var updateCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateUpdateCommandBuilder(fixture.GetParentDescriptor()))
            {
                fixture.Update(first, second);
                var valueProvider = new ClassValueProvider(new List<object> { first, second });

                updateCommandBuilder.Should().NotBeNull();

                valueProvider.MoveNext(); 
                var updateCommand = updateCommandBuilder.GetCommand(valueProvider);
                updateCommand.Should().NotBeNull();
                updateCommand.CommandText.Should().Be(fixture.UpdateStatement);
                updateCommand.Invoking(c => c.ExecuteNonQuery()).ShouldNotThrow();

                valueProvider.MoveNext();
                updateCommand = updateCommandBuilder.GetCommand(valueProvider);
                updateCommand.Should().NotBeNull();
                updateCommand.CommandText.Should().Be(fixture.UpdateStatement);
                updateCommand.Invoking(c => c.ExecuteNonQuery()).ShouldNotThrow();

            }

            using (var selectCommandBuilder = fixture.Connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(fixture.GetParentDescriptor()))
            {
                var selectCommand = selectCommandBuilder.GetCommand();

                using (var reader = selectCommand.ExecuteReader())
                {
                    var mapper = new DatabaseReaderMapper(fixture.Connector, fixture.GetParentDescriptor());
                    var results = mapper.Map(reader);

                    first = results[0];
                    second = results[1];

                    fixture.CheckUpdate(first, second);
                }
            }
            fixture.DropDatabase();
        }
    }
}
