using System;
using FluentAssertions;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Data.SqlServer;
using NUnit.Framework;
using Paradigm.ORM.Data.MySql;
using Paradigm.ORM.Data.Cassandra;

namespace Paradigm.ORM.Tests.Tests
{
    [TestFixture]
    public class DatabaseConnectorTest
    {
        private const string MySqlConnectionString = "Server=localhost;User=root;Password=Paradigm_Test_1234;Connection Timeout=3600";

        private const string PostgreSqlConnectionString = "Server=localhost;User Id=postgres;Password=Paradigm_Test_1234;Timeout=3";

        private const string SqlConnectionString = "Server=localhost;User=sa;Password=Paradigm_Test_1234;Connection Timeout=3600";

        private const string CqlConnectionString = "Contact Points=localhost;Port=9042;Default Keyspace=test;Username=root";

        [Order(1)]
        [TestCase(typeof(MySqlDatabaseConnector))]
        [TestCase(typeof(PostgreSqlDatabaseConnector))]
        [TestCase(typeof(SqlDatabaseConnector))]
        [TestCase(typeof(CqlDatabaseConnector))]
        public void ShouldCreateWithConnectionString(Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, "") as IDatabaseConnector)
            {
                connector.Should().NotBeNull();
            }
        }

        [Order(2)]
        [TestCase(typeof(MySqlDatabaseConnector))]
        [TestCase(typeof(PostgreSqlDatabaseConnector))]
        [TestCase(typeof(SqlDatabaseConnector))]
        [TestCase(typeof(CqlDatabaseConnector))]
        public void ShouldCreateWithConnectionStringAndBeInitialized(Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, "") as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.GetCommandBuilderFactory().Should().NotBeNull();
                connector.GetDbStringTypeConverter().Should().NotBeNull();
                connector.GetCommandFormatProvider().Should().NotBeNull();
                connector.GetDbTypeValueRangeProvider().Should().NotBeNull();
                connector.GetValueConverter().Should().NotBeNull();
                connector.GetSchemaProvider().Should().NotBeNull();
            }
        }

        [Order(3)]
        [TestCase(typeof(MySqlDatabaseConnector))]
        [TestCase(typeof(PostgreSqlDatabaseConnector))]
        [TestCase(typeof(SqlDatabaseConnector))]
        [TestCase(typeof(CqlDatabaseConnector))]
        public void ShouldCreateWithoutConnectionString(Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Invoking(x => x.IsOpen()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetCommandBuilderFactory()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetDbStringTypeConverter()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetCommandFormatProvider()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetDbTypeValueRangeProvider()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetValueConverter()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetSchemaProvider()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.CreateCommand()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.CreateTransaction()).Should().NotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.Close()).Should().NotThrow<OrmConnectorNotInitializedException>();
            }
        }

        [Order(4)]
        [TestCase(typeof(MySqlDatabaseConnector))]
        [TestCase(typeof(PostgreSqlDatabaseConnector))]
        [TestCase(typeof(SqlDatabaseConnector))]
        [TestCase(typeof(CqlDatabaseConnector))]
        public void ShouldInitialize(Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Invoking(x => x.Initialize("")).Should().NotThrow();
                connector.IsOpen().Should().BeFalse();

                connector.GetCommandBuilderFactory().Should().NotBeNull();
                connector.GetDbStringTypeConverter().Should().NotBeNull();
                connector.GetCommandFormatProvider().Should().NotBeNull();
                connector.GetDbTypeValueRangeProvider().Should().NotBeNull();
                connector.GetValueConverter().Should().NotBeNull();
                connector.GetSchemaProvider().Should().NotBeNull();
            }
        }

        [Order(5)]
        [TestCase(typeof(MySqlDatabaseConnector))]
        [TestCase(typeof(PostgreSqlDatabaseConnector))]
        [TestCase(typeof(SqlDatabaseConnector))]
        [TestCase(typeof(CqlDatabaseConnector))]
        public void ShouldThrowIfConnectionIsntPossible(Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, "") as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Invoking(x => x.Open()).Should().Throw<OrmCanNotOpenConnectionException>();
                connector.Awaiting(async x => await x.OpenAsync()).Should().Throw<OrmCanNotOpenConnectionException>();
            }
        }

        [Order(6)]
        [TestCase(MySqlConnectionString, typeof(MySqlDatabaseConnector))]
        [TestCase(PostgreSqlConnectionString, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(SqlConnectionString, typeof(SqlDatabaseConnector))]
        [TestCase(CqlConnectionString, typeof(CqlDatabaseConnector))]
        public void ShouldConnectAndClose(string connectorString, Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, connectorString) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Invoking(x => x.Open()).Should().NotThrow<OrmCanNotOpenConnectionException>();
                connector.Invoking(x => x.Close()).Should().NotThrow<OrmCanNotCloseConnectionException>();

                connector.Awaiting(async x => await x.OpenAsync()).Should().NotThrow<OrmCanNotOpenConnectionException>();
                connector.Awaiting(async x => await x.CloseAsync()).Should().NotThrow<OrmCanNotCloseConnectionException>();
            }
        }

        [Order(7)]
        [TestCase(MySqlConnectionString, typeof(MySqlDatabaseConnector))]
        [TestCase(PostgreSqlConnectionString, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(SqlConnectionString, typeof(SqlDatabaseConnector))]
        [TestCase(CqlConnectionString, typeof(CqlDatabaseConnector))]
        public void ShouldCreateACommand(string connectorString, Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, connectorString) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Open();

                using (var command = connector.CreateCommand())
                {
                    command.Should().BeAssignableTo<IDatabaseCommand>();
                }

                connector.Close();
            }
        }

        [Order(8)]
        [TestCase(MySqlConnectionString, typeof(MySqlDatabaseConnector))]
        [TestCase(PostgreSqlConnectionString, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(SqlConnectionString, typeof(SqlDatabaseConnector))]
        [TestCase(CqlConnectionString, typeof(CqlDatabaseConnector))]
        public void ShouldCreateATransaction(string connectorString, Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, connectorString) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Open();

                using (var transaction = connector.CreateTransaction())
                {
                    transaction.Should().BeAssignableTo<IDatabaseTransaction>();
                }

                connector.Close();
            }
        }
    }
}