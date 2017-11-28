using System;
using System.Collections.Generic;
using System.Data;

namespace Paradigm.ORM.Data.Database
{
    /// <summary>
    /// Provides an interface to send execution commands to the database.
    /// Can be either a query string, or a stored procedure name.
    /// </summary>
    public partial interface IDatabaseCommand : IDisposable
    {
        /// <summary>
        /// Gets or sets the SQL statement, table name or stored procedure to execute
        /// at the data source.
        /// </summary>
        /// <returns>
        /// The SQL statement or stored procedure to execute. The default is an
        /// empty string.
        /// </returns>
        string CommandText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how the CommandText
        /// property is to be interpreted.
        /// </summary>
        /// <returns>
        ///  One of the <see cref="System.Data.CommandType" /> values. The default is Text.
        /// </returns>
        CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command
        /// and generating an error.
        /// </summary>
        /// <returns>
        /// The time in seconds to wait for the command to execute. The default is 30 seconds.
        /// </returns>
        int CommandTimeout { get; set; }

        /// <summary>
        /// Gets the array of parameters of this command instance.
        /// </summary>
        /// <returns>
        /// The parameters of the SQL statement or stored procedure. The default
        /// is an empty collection.
        /// </returns>
        IEnumerable<IDbDataParameter> Parameters { get; }

        /// <summary>
        /// Sends the CommandText to the <see cref="IDatabaseConnector" />
        /// and builds a <see cref="IDatabaseReader"/>.
        /// </summary>
        /// <returns>A database reader object.</returns>
        IDatabaseReader ExecuteReader();

        /// <summary>
        /// Executes a SQL statement against the connection and returns the number
        /// of rows affected.
        /// </summary>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        int ExecuteNonQuery();

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the result set, or a null reference
        /// if the result set is empty.
        /// </returns>
        object ExecuteScalar();

        /// <summary>
        /// Adds an existing parameter to the command.
        /// </summary>
        /// <param name="parameter">Parameter to add.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(IDataParameter parameter);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, Type type);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, Type type, long size);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, Type type, byte precision, byte scale);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, Type type, long size, byte precision, byte scale);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <param name="value">Parameter value</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, Type type, long size, byte precision, byte scale, object value);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type, bool isNullable);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type, long size);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type, int size, bool isNullable);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type, byte precision, byte scale);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type, long size, byte precision, byte scale);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type, byte precision, byte scale, bool isNullable);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type, long size, byte precision, byte scale, bool isNullable);

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>The reference of the parameter recently added.</returns>
        IDataParameter AddParameter(string name, DbType type, long size, byte precision, byte scale, bool isNullable, object value);

        /// <summary>
        /// Gets a parameter by index.
        /// </summary>
        /// <param name="index">Parameter index.</param>
        /// <returns>The reference of the parameter.</returns>
        IDataParameter GetParameter(int index);

        /// <summary>
        /// Gets a parameter by name.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <returns>The reference of the parameter.</returns>
        IDataParameter GetParameter(string name);

        /// <summary>
        /// Clears the parameters.
        /// </summary>
        void ClearParameters();
    }
}