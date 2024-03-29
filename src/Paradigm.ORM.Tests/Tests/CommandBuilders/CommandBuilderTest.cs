﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.ValueProviders;
using Paradigm.ORM.Tests.Fixtures;
using Paradigm.ORM.Tests.Fixtures.MySql;
using Paradigm.ORM.Tests.Fixtures.PostgreSql;
using Paradigm.ORM.Tests.Fixtures.Sql;
using NUnit.Framework;
using Paradigm.ORM.Tests.Fixtures.Cql;

namespace Paradigm.ORM.Tests.Tests.CommandBuilders
{
    [TestFixture]
    public class CommandBuilderTest
    {
        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.SimpleTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.SimpleTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.SimpleTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.SimpleTable))]
        public void ShouldCreateASelectCommand(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);

            var selectCommand = commandBuilderFactory.CreateSelectCommandBuilder(tableDescription);
            selectCommand.GetCommand().CommandText.Should().Be(fixture.SelectQuery);
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.SimpleTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.SimpleTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.SimpleTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.SimpleTable))]
        public void ShouldCreateASelectCommandWithAWhereClause(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);
            var selectCommand = commandBuilderFactory.CreateSelectCommandBuilder(tableDescription);
            selectCommand.GetCommand(fixture.SelectWhereClause).CommandText.Should().Be(fixture.SelectWithWhereQuery);
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.SimpleTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.SimpleTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.SimpleTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.SimpleTable))]
        public void ShouldCreateASelectOneCommand(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);

            var selectCommand = commandBuilderFactory.CreateSelectOneCommandBuilder(tableDescription);

            var command = selectCommand.GetCommand(723);
            var parameters = command.Parameters.ToList();
            command.CommandText.Should().Be(fixture.SelectOneQuery);
            parameters.Should().HaveCount(1);
            parameters[0].Value.Should().Be(723);
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.TwoPrimaryKeyTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.TwoPrimaryKeyTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.TwoPrimaryKeyTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.TwoPrimaryKeyTable))]
        public void ShouldCreateASelectOneCommandWithMultipleIds(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);

            var selectCommand = commandBuilderFactory.CreateSelectOneCommandBuilder(tableDescription);

            var command = fixtureType == typeof(CqlCommandBuilderFixture)
                ? selectCommand.GetCommand(723, 23, DateTimeOffset.Now)
                : selectCommand.GetCommand(723, 23);

            var parameters = command.Parameters.ToList();
            command.CommandText.Should().Be(fixture.SelectWithTwoPrimaryKeysQuery);
            parameters.Should().HaveCount(fixtureType == typeof(CqlCommandBuilderFixture) ? 3 : 2);
            parameters[0].Value.Should().Be(723);
            parameters[1].Value.Should().Be(23);
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.TwoPrimaryKeyTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.TwoPrimaryKeyTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.TwoPrimaryKeyTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.TwoPrimaryKeyTable))]
        public void ShouldFailIWhenCreatingASelectOneCommandWithoutParameters(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);
            var selectCommand = commandBuilderFactory.CreateSelectOneCommandBuilder(tableDescription);

            Action select = () => selectCommand.GetCommand(null);
            select.Should().Throw<ArgumentNullException>();
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.TwoPrimaryKeyTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.TwoPrimaryKeyTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.TwoPrimaryKeyTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.TwoPrimaryKeyTable))]
        public void ShouldFailWhenCreatingASelectOneCommandWithTooManyParameters(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);
            var selectCommand = commandBuilderFactory.CreateSelectOneCommandBuilder(tableDescription);

            Action select = () => selectCommand.GetCommand(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            select.Should().Throw<ArgumentException>();
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.SimpleTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.SimpleTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.SimpleTable))]
        public void ShouldCreateALastIdCommand(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);

            var lastIdCommand = commandBuilderFactory.CreateLastInsertIdCommandBuilder(tableDescription);
            lastIdCommand.GetCommand().CommandText.Should().Be(fixture.LastInsertIdQuery);
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.SimpleTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.SimpleTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.SimpleTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.SimpleTable))]
        public void ShouldCreateAnInsertCommand(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);
            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { fixture.Entity1 });
            valueProvider.MoveNext();

            var insertCommand = commandBuilderFactory.CreateInsertCommandBuilder(tableDescription);

            var command = insertCommand.GetCommand(valueProvider);
            var parameters = command.Parameters.ToList();
            command.CommandText.Should().Be(fixture.InsertQuery);

            if (fixtureType == typeof(CqlCommandBuilderFixture))
            {
                parameters.Should().HaveCount(5);
                parameters.Sort((x, y) => string.CompareOrdinal(x.ParameterName, y.ParameterName));
                parameters[0].Value.Should().Be(3600m);
                parameters[3].Value.Should().Be(true);
                parameters[4].Value.Should().Be("John Doe");
            }
            else
            {
                parameters.Should().HaveCount(4);
                parameters.Sort((x, y) => string.CompareOrdinal(x.ParameterName, y.ParameterName));
                parameters[0].Value.Should().Be(3600m);
                parameters[1].Value.Should().Be(new DateTime(2017, 5, 23, 13, 55, 43, 0));
                parameters[2].Value.Should().Be(true);
                parameters[3].Value.Should().Be("John Doe");
            }
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.SimpleTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.SimpleTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.SimpleTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.SimpleTable))]
        public void ShouldCreateADeleteCommandForASingleEntityWithASingleKey(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);
            var entitiesToDelete = new[] { fixture.Entity1 };
            var valueProvider = new ClassValueProvider(fixture.Connector, entitiesToDelete.Cast<object>().ToList());

            var deleteCommand = commandBuilderFactory.CreateDeleteCommandBuilder(tableDescription);
            deleteCommand.GetCommand(valueProvider).CommandText.Should().Be(fixture.DeleteOneEntityQuerySingleKey);
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.TwoPrimaryKeyTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.TwoPrimaryKeyTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.TwoPrimaryKeyTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.TwoPrimaryKeyTable))]
        public void ShouldCreateADeleteCommandForASingleEntityWithMultipleKeys(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);
            var entitiesToDelete = new[] { fixture.TwoPrimaryKeyEntity1 };
            var valueProvider = new ClassValueProvider(fixture.Connector, entitiesToDelete.Cast<object>().ToList());

            var deleteCommand = commandBuilderFactory.CreateDeleteCommandBuilder(tableDescription);
            deleteCommand.GetCommand(valueProvider).CommandText.Should().Be(fixture.DeleteOneEntityQueryMultipleKey);
        }

        [TestCase(typeof(MySqlCommandBuilderFixture), typeof(Mocks.MySql.SimpleTable))]
        [TestCase(typeof(SqlCommandBuilderFixture), typeof(Mocks.Sql.SimpleTable))]
        [TestCase(typeof(PostgreSqlCommandBuilderFixture), typeof(Mocks.PostgreSql.SimpleTable))]
        [TestCase(typeof(CqlCommandBuilderFixture), typeof(Mocks.Cql.SimpleTable))]
        public void ShouldCreateAUpdateCommand(Type fixtureType, Type tableDescriptorType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBuilderFixtureBase;
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableDescription = DescriptorCache.Instance.GetTableTypeDescriptor(tableDescriptorType);

            var entityToUpdate = fixture.Entity1;
            entityToUpdate.Name = "John Doe Junior";
            entityToUpdate.Amount = 7200m;

            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { entityToUpdate });
            valueProvider.MoveNext();

            var updateCommand = commandBuilderFactory.CreateUpdateCommandBuilder(tableDescription);
            var command = updateCommand.GetCommand(valueProvider);
            var parameters = command.Parameters.ToList();
            command.CommandText.Should().Be(fixture.UpdateQuery);

            if (fixtureType == typeof(CqlCommandBuilderFixture))
            {
                parameters.Should().HaveCount(5);
                parameters.Sort((x, y) => string.CompareOrdinal(x.ParameterName, y.ParameterName));
                parameters[0].Value.Should().Be(7200m);
                parameters[2].Value.Should().Be(fixture.Entity1.Id);
                parameters[3].Value.Should().Be(fixture.Entity1.IsActive);
                parameters[4].Value.Should().Be("John Doe Junior");
            }
            else
            {
                parameters.Should().HaveCount(5);
                parameters.Sort((x, y) => string.CompareOrdinal(x.ParameterName, y.ParameterName));
                parameters[0].Value.Should().Be(7200m);
                parameters[1].Value.Should().Be(fixture.Entity1.CreatedDate);
                parameters[2].Value.Should().Be(fixture.Entity1.Id);
                parameters[3].Value.Should().Be(fixture.Entity1.IsActive);
                parameters[4].Value.Should().Be("John Doe Junior");
            }
        }
    }
}
