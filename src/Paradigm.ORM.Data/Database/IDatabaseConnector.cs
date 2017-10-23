using System;
using System.Data;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database.Schema;

namespace Paradigm.ORM.Data.Database
{
    /// <summary>
    /// Provides an interface to connecto to a database server.
    /// </summary>
    public partial interface IDatabaseConnector : IDisposable
    {
        /// <summary>
        /// Gets the time to wait while trying to establish a connection before terminating
        /// the attempt and generating an error.
        /// </summary>
        ///<returns>
        /// The time (in seconds) to wait for a connection to open. The default value is
        /// 15 seconds.
        ///</returns>
        int ConnectionTimeout { get; }

        /// <summary>
        /// Gets the database configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        IDatabaseConfiguration Configuration { get; }

        /// <summary>
        /// Initializes the connection against the database.
        /// </summary>
        /// <param name="connectionString">A connection string containing information to connect to a given database.</param>
        void Initialize(string connectionString = null);

        /// <summary>
        /// Opens the conection to a database.
        /// </summary>
        void Open();

        /// <summary>
        /// Closes a previously opened connection to a database.
        /// </summary>
        void Close();

        /// <summary>
        /// Indicates if the connection is currently opened.
        /// </summary>
        /// <returns>True if the connection is opened, false otherwise.</returns>
        bool IsOpen();

        /// <summary>
        /// Creates a new database transaction.
        /// </summary>
        /// <returns>A new transaction</returns>
        IDatabaseTransaction CreateTransaction();

        /// <summary>
        /// Creates a new database transaction.
        /// </summary>
        /// <param name="isolationLevel">Specifies the transaction isolation level.</param>
        /// <returns>A new transaction.</returns>
        IDatabaseTransaction CreateTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Creates a new database command.
        /// </summary>
        /// <returns>A new command.</returns>
        IDatabaseCommand CreateCommand();

        /// <summary>
        /// Gets a command builder factory.
        /// </summary>
        /// <remarks>
        /// The command builder factory creates command builders for
        /// standard crud actions over a given data type.
        /// </remarks>
        /// <returns>The command builder factory related to this database type.</returns>
        ICommandBuilderFactory GetCommandBuilderFactory();

        /// <summary>
        /// Get a command text format provider.
        /// </summary>
        /// <remarks>
        /// The command formatter gives basic functionality to format
        /// sql queries for each database type.
        /// </remarks>
        /// <returns>The command formatted realted to this database type.</returns>
        ICommandFormatProvider GetCommandFormatProvider();

        /// <summary>
        /// Gets a database schema provider.
        /// </summary>
        /// <remarks>
        /// The schema provider, allows to retrieve schema information about database objects like tables, views, stored procedures, etc.
        /// </remarks>
        /// <returns>A schema provider related to this database connection.</returns>
        ISchemaProvider GetSchemaProvider();

        /// <summary>
        /// Gets a string type converter.
        /// </summary>
        /// <remarks>
        /// This converter can convert back and forth from a database type in string format, to a .NET type.
        /// </remarks>
        /// <example>
        /// For the string "int" the converter will return <see cref="Int32" />.
        /// For the string "text" the converter will return <see cref="String" />.
        /// </example>
        /// <returns>A database type converter.</returns>
        IDbStringTypeConverter GetDbStringTypeConverter();

        /// <summary>
        /// Gets the value converter.
        /// </summary>
        /// <remarks>
        /// This converter can convert from database objects to specific .net types.
        /// </remarks>
        /// <returns>A value converter.</returns>
        IValueConverter GetValueConverter();

        /// <summary>
        /// Gets the database type size provider.
        /// </summary>
        /// <remarks>
        /// A database type size provider retrieves minimum and maximum values for regular sql types
        /// like tinyint, smallint, int, etc.
        /// </remarks>
        /// <returns></returns>
        IDbTypeValueRangeProvider GetDbTypeValueRangeProvider();
    }
}