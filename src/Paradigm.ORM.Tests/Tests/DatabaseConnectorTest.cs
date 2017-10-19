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
        private const string MySqlConnectionString = "Server=192.168.2.160;User=test;Password=test1234;Connection Timeout=3600";

        private const string PostgreSqlConnectionString = "Server=192.168.2.160;User Id=test;Password=test1234;Timeout=3";

        private const string SqlConnectionString = "Server=192.168.2.160;User=test;Password=test1234;Connection Timeout=3600";

        private const string CqlConnectionString = "Contact Points=192.168.2.240;Port=9042";

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

                connector.Invoking(x => x.IsOpen()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetCommandBuilderFactory()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetDbStringTypeConverter()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetCommandFormatProvider()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetDbTypeValueRangeProvider()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetValueConverter()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.GetSchemaProvider()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.CreateCommand()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.CreateTransaction()).ShouldNotThrow<OrmConnectorNotInitializedException>();
                connector.Invoking(x => x.Close()).ShouldNotThrow<OrmConnectorNotInitializedException>();
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

                connector.Invoking(x => x.Initialize("")).ShouldNotThrow();
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

                connector.Invoking(x => x.Open()).ShouldThrow<OrmCanNotOpenConnectionException>();
                connector.Awaiting(async x => await x.OpenAsync()).ShouldThrow<OrmCanNotOpenConnectionException>();
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

                connector.Invoking(x => x.Open()).ShouldNotThrow<OrmCanNotOpenConnectionException>();
                connector.Invoking(x => x.Close()).ShouldNotThrow<OrmCanNotCloseConnectionException>();

                connector.Awaiting(async x => await x.OpenAsync()).ShouldNotThrow<OrmCanNotOpenConnectionException>();
                connector.Awaiting(async x => await x.CloseAsync()).ShouldNotThrow<OrmCanNotCloseConnectionException>();
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