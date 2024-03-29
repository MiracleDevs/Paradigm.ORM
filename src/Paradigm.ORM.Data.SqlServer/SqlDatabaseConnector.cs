using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Database.Schema;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Logging;
using Paradigm.ORM.Data.SqlServer.CommandBuilders;
using Paradigm.ORM.Data.SqlServer.Converters;
using Paradigm.ORM.Data.SqlServer.Schema;

namespace Paradigm.ORM.Data.SqlServer
{
    /// <summary>
    /// Provides a connection to a SQL Server database.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.IDatabaseConnector" />
    public partial class SqlDatabaseConnector : IDatabaseConnector
    {
        #region Properties

        /// <summary>
        /// Gets the time to wait while trying to establish a connection before terminating
        /// the attempt and generating an error.
        /// </summary>
        ///<returns>
        /// The time (in seconds) to wait for a connection to open. The default value is
        /// 15 seconds.
        ///</returns>
        public int ConnectionTimeout => this.Connection.ConnectionTimeout;

        /// <summary>
        /// Gets the database configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IDatabaseConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the active transaction.
        /// </summary>
        /// <value>
        /// The active transaction.
        /// </value>
        internal SqlDatabaseTransaction ActiveTransaction => this.Transactions.Any() ? this.Transactions.Peek() : null;

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        private SqlConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets the format provider.
        /// </summary>
        /// <value>
        /// The format provider.
        /// </value>
        private Lazy<ICommandFormatProvider> FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets the schema provider.
        /// </summary>
        /// <value>
        /// The schema provider.
        /// </value>
        private Lazy<ISchemaProvider> SchemaProvider { get; set; }

        /// <summary>
        /// Gets or sets the command builder factory.
        /// </summary>
        /// <value>
        /// The command builder factory.
        /// </value>
        private Lazy<ICommandBuilderFactory> CommandBuilderFactory { get; set; }

        /// <summary>
        /// Gets or sets the database string type converter.
        /// </summary>
        /// <value>
        /// The database string type converter.
        /// </value>
        private Lazy<IDbStringTypeConverter> DbStringTypeConverter { get; set; }

        /// <summary>
        /// Gets or sets the value converter.
        /// </summary>
        /// <value>
        /// The value converter.
        /// </value>
        private Lazy<IValueConverter> ValueConverter { get; set; }

        /// <summary>
        /// Gets or sets the value provider.
        /// </summary>
        /// <value>
        /// The value provider.
        /// </value>
        private Lazy<SqlDbTypeValueRangeProvider> ValueProvider { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>
        /// The transactions.
        /// </value>
        private Stack<SqlDatabaseTransaction> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the log provider.
        /// </summary>
        /// <value>
        /// The log provider.
        /// </value>
        internal ILogProvider LogProvider { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabaseConnector"/> class.
        /// </summary>
        public SqlDatabaseConnector()
        {
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabaseConnector"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlDatabaseConnector(string connectionString)
        {
            this.Initialize(connectionString);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Connection?.Dispose();

            if (this.Transactions != null)
            {
                foreach (var transaction in this.Transactions.ToList())
                    transaction?.Dispose();

                Transactions.Clear();
                Transactions = null;
            }

            this.Connection = null;
            this.FormatProvider = null;
            this.SchemaProvider = null;
            this.CommandBuilderFactory = null;
            this.DbStringTypeConverter = null;
            this.ValueConverter = null;
            this.ValueProvider = null;
        }

        /// <summary>
        /// Pops the last transaction from the transactions stack.
        /// </summary>
        internal void PopTransaction()
        {
            this.ThrowIfNull();
            this.ThrowIfFails<OrmTransactionStackEmptyException>(() => this.Transactions.Pop(), OrmTransactionStackEmptyException.DefaultMessage);
        }

        /// <summary>
        /// Initializes the connection against the database.
        /// </summary>
        /// <param name="connectionString">A connection string containing information to connect to a given database.</param>
        public void Initialize(string connectionString = null)
        {
            this.Dispose();

            this.Configuration = new DatabaseConfiguration(int.MaxValue, 2100, int.MaxValue);
            this.FormatProvider = new Lazy<ICommandFormatProvider>(() => new SqlCommandFormatProvider(), true);
            this.SchemaProvider = new Lazy<ISchemaProvider>(() => new SqlSchemaProvider(this), true);
            this.CommandBuilderFactory = new Lazy<ICommandBuilderFactory>(() => new SqlCommandBuilderFactory(this), true);
            this.DbStringTypeConverter = new Lazy<IDbStringTypeConverter>(() => new SqlDbStringTypeConverter(), true);
            this.ValueConverter = new Lazy<IValueConverter>(() => new SqlValueConverter(), true);
            this.ValueProvider = new Lazy<SqlDbTypeValueRangeProvider>(() => new SqlDbTypeValueRangeProvider(), true);
            this.Transactions = new Stack<SqlDatabaseTransaction>();

            this.Connection = connectionString == null ? new SqlConnection() : new SqlConnection(connectionString);
        }

        /// <summary>
        /// Opens the connection to a database.
        /// </summary>
        public void Open()
        {
            this.ThrowIfNull();
            this.ThrowIfFails<OrmCanNotOpenConnectionException>(() => this.Connection.Open(), OrmCanNotOpenConnectionException.DefaultMessage);
        }

        /// <summary>
        /// Closes a previously opened connection to a database.
        /// </summary>
        public void Close()
        {
            this.ThrowIfNull();
            this.ThrowIfFails<OrmCanNotCloseConnectionException>(() => this.Connection.Close(), OrmCanNotCloseConnectionException.DefaultMessage);
        }

        /// <summary>
        /// Indicates if the connection is currently opened.
        /// </summary>
        /// <returns>
        /// True if the connection is opened, false otherwise.
        /// </returns>
        public bool IsOpen()
        {
            this.ThrowIfNull();
            return this.Connection.State == ConnectionState.Open;
        }

        /// <summary>
        /// Creates a new database transaction.
        /// </summary>
        /// <returns>
        /// A new <see cref="SqlDatabaseTransaction">transaction</see>.
        /// </returns>
        public IDatabaseTransaction CreateTransaction()
        {
            return this.CreateTransaction(IsolationLevel.Serializable);
        }

        /// <summary>
        /// Creates a new database transaction.
        /// </summary>
        /// <param name="isolationLevel">Specifies the transaction isolation level.</param>
        /// <returns>
        /// A new <see cref="SqlDatabaseTransaction">transaction</see>.
        /// </returns>
        public IDatabaseTransaction CreateTransaction(IsolationLevel isolationLevel)
        {
            this.ThrowIfNull();

            if (!this.IsOpen())
                this.Open();

            return this.ThrowIfFails<OrmConnectorException, IDatabaseTransaction>(() =>
            {
                var transaction = new SqlDatabaseTransaction(this.Connection.BeginTransaction(isolationLevel), this);
                this.Transactions.Push(transaction);
                return transaction;

            }, "Can not create the transaction.");
        }

        /// <summary>
        /// Creates a new database command.
        /// </summary>
        /// <returns>
        /// A new <see cref="SqlDatabaseCommand">command</see>.
        /// </returns>
        /// <exception cref="System.Exception">Couldn't create the command.</exception>
        public IDatabaseCommand CreateCommand()
        {
            this.ThrowIfNull();

            return this.ThrowIfFails<OrmConnectorException, IDatabaseCommand>(() =>
            {
                var command = this.Connection.CreateCommand();
                command.Connection = Connection;
                command.CommandTimeout = this.ConnectionTimeout;
                return new SqlDatabaseCommand(command, this);

            }, "Can not create the command.");
        }

        /// <summary>
        /// Gets a command builder factory.
        /// </summary>
        /// <returns>
        /// The command builder factory related to this database type.
        /// </returns>
        /// <remarks>
        /// The command builder factory creates command builders for
        /// standard crud actions over a given data type.
        /// </remarks>
        /// <see cref="SqlCommandBuilderFactory"/>
        public ICommandBuilderFactory GetCommandBuilderFactory()
        {
            this.ThrowIfNull();
            return this.CommandBuilderFactory.Value;
        }

        /// <summary>
        /// Get a command text format provider.
        /// </summary>
        /// <returns>
        /// The command formatted related to this database type.
        /// </returns>
        /// <remarks>
        /// The command formatter gives basic functionality to format
        /// sql queries for each database type.
        /// </remarks>
        /// <see cref="SqlCommandFormatProvider"/>
        public ICommandFormatProvider GetCommandFormatProvider()
        {
            this.ThrowIfNull();
            return this.FormatProvider.Value;
        }

        /// <summary>
        /// Get a database schema provider.
        /// </summary>
        /// <returns>
        /// A schema provider related to this database connection.
        /// </returns>
        /// <remarks>
        /// The schema provider, allows to retrieve schema information about database objects like tables, views, stored procedures, etc.
        /// </remarks>
        /// <see cref="SqlSchemaProvider"/>
        public ISchemaProvider GetSchemaProvider()
        {
            this.ThrowIfNull();
            return this.SchemaProvider.Value;
        }

        /// <summary>
        /// Gets a string type converter.
        /// </summary>
        /// <returns>
        /// A database type converter.
        /// </returns>
        /// <remarks>
        /// This converter can convert back and forth from a database type in string format, to a .NET type.
        /// </remarks>
        /// <example>
        /// For the string "int" the converter will return <see cref="Int32" />.
        /// For the string "text" the converter will return <see cref="String" />.
        /// </example>
        public IDbStringTypeConverter GetDbStringTypeConverter()
        {
            this.ThrowIfNull();
            return this.DbStringTypeConverter.Value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the value converter.
        /// </summary>
        /// <returns>
        /// A value converter.
        /// </returns>
        /// <remarks>
        /// This converter can convert from database objects to specific .net types.
        /// </remarks>
        public IValueConverter GetValueConverter()
        {
            this.ThrowIfNull();
            return this.ValueConverter.Value;
        }

        /// <summary>
        /// Gets the database type size provider.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// A database type size provider retrieves minimum and maximum values for regular sql types
        /// like tinyint, smallint, int, etc.
        /// </remarks>
        public IDbTypeValueRangeProvider GetDbTypeValueRangeProvider()
        {
            this.ThrowIfNull();
            return this.ValueProvider.Value;
        }

        /// <summary>
        /// Enables the logs.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void EnableLogs(ILogProvider provider)
        {
            this.ThrowIfNull();
            this.LogProvider = provider;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Throws if the connection is null.
        /// </summary>
        /// <exception cref="System.Exception">MySql Connection is not initialized.</exception>
        private void ThrowIfNull()
        {
            if (this.Connection == null)
            {
                throw new OrmConnectorNotInitializedException();
            }
        }

        #endregion
    }
}