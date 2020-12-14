using System;
using System.Threading.Tasks;
using FluentAssertions;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Data.SqlServer;
using NUnit.Framework;
using Paradigm.ORM.Data.MySql;
using Paradigm.ORM.Data.Cassandra;
using Paradigm.ORM.Tests.Fixtures;

namespace Paradigm.ORM.Tests.Tests
{
    [TestFixture]
    public class DatabaseConnectorTest
    {
        [Order(1)]
        [TestCase(ConnectionStrings.MySql, typeof(MySqlDatabaseConnector))]
        [TestCase(ConnectionStrings.PSql, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(ConnectionStrings.MsSql, typeof(SqlDatabaseConnector))]
        [TestCase(ConnectionStrings.Cql, typeof(CqlDatabaseConnector))]
        public void ShouldCreateWithConnectionString(string connectionString, Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, connectionString) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();
            }
        }

        [Order(2)]
        [TestCase(ConnectionStrings.MySql, typeof(MySqlDatabaseConnector))]
        [TestCase(ConnectionStrings.PSql, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(ConnectionStrings.MsSql, typeof(SqlDatabaseConnector))]
        [TestCase(ConnectionStrings.Cql, typeof(CqlDatabaseConnector))]
        public void ShouldCreateWithConnectionStringAndBeInitialized(string connectionString, Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, connectionString) as IDatabaseConnector)
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
        [TestCase(ConnectionStrings.MySql, typeof(MySqlDatabaseConnector))]
        [TestCase(ConnectionStrings.PSql, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(ConnectionStrings.MsSql, typeof(SqlDatabaseConnector))]
        [TestCase(ConnectionStrings.Cql, typeof(CqlDatabaseConnector))]
        public void ShouldInitialize(string connectionString, Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Invoking(x => x.Initialize(connectionString)).Should().NotThrow();
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
        [TestCase("Server=1.1.1.1", typeof(MySqlDatabaseConnector))]
        [TestCase("Server=1.1.1.1", typeof(PostgreSqlDatabaseConnector))]
        [TestCase("Server=1.1.1.1", typeof(SqlDatabaseConnector))]
        [TestCase("Contact Points=1.1.1.1", typeof(CqlDatabaseConnector))]
        public void ShouldThrowIfConnectionIsntPossible(string connectionString, Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, connectionString) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Invoking(x => x.Open()).Should().Throw<OrmCanNotOpenConnectionException>();
                connector.Awaiting((Func<IDatabaseConnector, Task>)(async x => await x.OpenAsync())).Should().Throw<OrmCanNotOpenConnectionException>();
            }
        }

        [Order(6)]
        [TestCase(ConnectionStrings.MySql, typeof(MySqlDatabaseConnector))]
        [TestCase(ConnectionStrings.PSql, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(ConnectionStrings.MsSql, typeof(SqlDatabaseConnector))]
        [TestCase(ConnectionStrings.Cql, typeof(CqlDatabaseConnector))]
        public void ShouldConnectAndClose(string connectorString, Type connectorType)
        {
            using (var connector = Activator.CreateInstance(connectorType, connectorString) as IDatabaseConnector)
            {
                connector.Should().NotBeNull();

                connector.Invoking(x => x.Open()).Should().NotThrow<OrmCanNotOpenConnectionException>();
                connector.Invoking(x => x.Close()).Should().NotThrow<OrmCanNotCloseConnectionException>();

                connector.Awaiting((Func<IDatabaseConnector, Task>)(async x => await x.OpenAsync())).Should().NotThrow<OrmCanNotOpenConnectionException>();
                connector.Awaiting((Func<IDatabaseConnector, Task>)(async x => await x.CloseAsync())).Should().NotThrow<OrmCanNotCloseConnectionException>();
            }
        }

        [Order(7)]
        [TestCase(ConnectionStrings.MySql, typeof(MySqlDatabaseConnector))]
        [TestCase(ConnectionStrings.PSql, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(ConnectionStrings.MsSql, typeof(SqlDatabaseConnector))]
        [TestCase(ConnectionStrings.Cql, typeof(CqlDatabaseConnector))]
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
        [TestCase(ConnectionStrings.MySql, typeof(MySqlDatabaseConnector))]
        [TestCase(ConnectionStrings.PSql, typeof(PostgreSqlDatabaseConnector))]
        [TestCase(ConnectionStrings.MsSql, typeof(SqlDatabaseConnector))]
        [TestCase(ConnectionStrings.Cql, typeof(CqlDatabaseConnector))]
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