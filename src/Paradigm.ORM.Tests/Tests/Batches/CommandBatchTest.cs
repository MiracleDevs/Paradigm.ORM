using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Paradigm.ORM.Data.Batching;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.ValueProviders;
using Paradigm.ORM.Tests.Fixtures;
using Paradigm.ORM.Tests.Fixtures.Cql;
using Paradigm.ORM.Tests.Fixtures.MySql;
using Paradigm.ORM.Tests.Fixtures.PostgreSql;
using Paradigm.ORM.Tests.Fixtures.Sql;
using Paradigm.ORM.Tests.Mocks.Batches;

namespace Paradigm.ORM.Tests.Tests.Batches
{
    [TestFixture]
    public class CommandBatchTest
    {
        [TestCase(typeof(MySqlCommandBatchFixture))]
        [TestCase(typeof(SqlCommandBatchFixture))]
        [TestCase(typeof(PostgreSqlCommandBatchFixture))]
        [TestCase(typeof(CqlCommandBatchFixture))]
        public void ShouldCreateCommandBatch(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBatchFixtureBase;
            var commandBatch = new CommandBatch(fixture.Connector);
            commandBatch.Should().NotBeNull();
        }

        [TestCase(typeof(MySqlCommandBatchFixture))]
        [TestCase(typeof(SqlCommandBatchFixture))]
        [TestCase(typeof(PostgreSqlCommandBatchFixture))]
        [TestCase(typeof(CqlCommandBatchFixture))]
        public void ShouldAddACommandToTheCommandBatch(Type fixtureType)
        {
            var fixture = Activator.CreateInstance(fixtureType) as CommandBatchFixtureBase;
            var commandBatch = new CommandBatch(fixture.Connector);
            var commandBuilderFactory = fixture.Connector.GetCommandBuilderFactory();
            var tableTypeDescriptor = DescriptorCache.Instance.GetTableTypeDescriptor(typeof(BatchMock));
            var valueProvider = new ClassValueProvider(fixture.Connector, new List<object> { fixture.CreateMock(), fixture.CreateMock() });

            commandBatch.Add(new CommandBatchStep(commandBuilderFactory.CreateInsertCommandBuilder(tableTypeDescriptor).GetCommand(valueProvider)));
            commandBatch.Add(new CommandBatchStep(commandBuilderFactory.CreateInsertCommandBuilder(tableTypeDescriptor).GetCommand(valueProvider)));

            commandBatch.GetCommand().CommandText.Should().Be(fixture.CommandBatchText);
        }
    }
}