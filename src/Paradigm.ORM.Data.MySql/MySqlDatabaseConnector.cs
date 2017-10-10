using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Database.Schema;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.MySql.CommandBuilders;
using Paradigm.ORM.Data.MySql.Converters;
using Paradigm.ORM.Data.MySql.Schema;
using MySql.Data.MySqlClient;

namespace Paradigm.ORM.Data.MySql
{
    /// <summary>
    /// Provides a connection to a MySql Server database.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.IDatabaseConnector" />
    public partial class MySqlDatabaseConnector : IDatabaseConnector
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
        /// Gets the active transaction.
        /// </summary>
        /// <value>
        /// The active transaction.
        /// </value>
        internal MySqlDatabaseTransaction ActiveTransaction => this.Transactions.Any() ? this.Transactions.Peek() : null;

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        private MySqlConnection Connection { get; set; }

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
        /// Gets or sets the value provider.
        /// </summary>
        /// <value>
        /// The value provider.
        /// </value>
        private Lazy<MySqlDbTypeValueRangeProvider> ValueProvider { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>
        /// The transactions.
        /// </value>
        private Stack<MySqlDatabaseTransaction> Transactions { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDatabaseConnector"/> class.
        /// </summary>
        public MySqlDatabaseConnector()
        {
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDatabaseConnector"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MySqlDatabaseConnector(string connectionString)
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

            if (this.SchemaProvider?.IsValueCreated ?? false)
                this.SchemaProvider.Value?.Dispose();

            if (this.Transactions != null)
            {
                foreach (var transaction in this.Transactions)
                    transaction?.Dispose();

                Transactions.Clear();
                Transactions = null;
            }

            this.Connection = null;
            this.FormatProvider = null;
            this.SchemaProvider = null;
            this.CommandBuilderFactory = null;
            this.DbStringTypeConverter = null;
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

            this.FormatProvider = new Lazy<ICommandFormatProvider>(() => new MySqlCommandFormatProvider(), true);
            this.SchemaProvider = new Lazy<ISchemaProvider>(() => new MySqlSchemaProvider(this), true);
            this.CommandBuilderFactory = new Lazy<ICommandBuilderFactory>(() => new MySqlCommandBuilderFactory(this), true);
            this.DbStringTypeConverter = new Lazy<IDbStringTypeConverter>(() => new MySqlDbStringTypeConverter(), true);
            this.ValueProvider = new Lazy<MySqlDbTypeValueRangeProvider>(() => new MySqlDbTypeValueRangeProvider(), true);
            this.Transactions = new Stack<MySqlDatabaseTransaction>();

            this.Connection = connectionString == null ? new MySqlConnection() : new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Opens the conection to a database.
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
        /// A new <see cref="MySqlDatabaseTransaction">transaction</see>.
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
        /// A new <see cref="MySqlDatabaseTransaction">transaction</see>.
        /// </returns>
        public IDatabaseTransaction CreateTransaction(IsolationLevel isolationLevel)
        {
            this.ThrowIfNull();

            return this.ThrowIfFails<OrmConnectorException, IDatabaseTransaction>(() =>
            {
                var transaction = new MySqlDatabaseTransaction(this.Connection.BeginTransaction(isolationLevel) as MySqlTransaction, this);
                this.Transactions.Push(transaction);
                return transaction;

            }, "Can not create the transaction.");
        }

        /// <summary>
        /// Creates a new database command.
        /// </summary>
        /// <returns>
        /// A new <see cref="MySqlDatabaseCommand">command</see>.
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
                return new MySqlDatabaseCommand(command as MySqlCommand, this);

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
        /// <see cref="MySqlCommandBuilderFactory"/>
        public ICommandBuilderFactory GetCommandBuilderFactory()
        {
            this.ThrowIfNull();
            return this.CommandBuilderFactory.Value;
        }

        /// <summary>
        /// Get a command text format provider.
        /// </summary>
        /// <returns>
        /// The command formatted realted to this database type.
        /// </returns>
        /// <remarks>
        /// The command formatter gives basic functionality to format
        /// sql queries for each database type.
        /// </remarks>
        /// <see cref="MySqlCommandFormatProvider"/>
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
        /// <see cref="MySqlSchemaProvider"/>
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