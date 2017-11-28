using System;
using System.Collections.Generic;
using System.Data;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;
using MySql.Data.MySqlClient;

namespace Paradigm.ORM.Data.MySql
{

    /// <summary>
    /// Provides a way to execute commands on a MySql Server Database.
    /// </summary>
    /// <seealso cref="IDatabaseCommand" />
    internal partial class MySqlDatabaseCommand : IDatabaseCommand
    {
        #region Properties

        /// <summary>
        /// Gets or sets the internal MySqlCommand instance.
        /// </summary>
        private MySqlCommand Command { get; set; }

        /// <summary>
        /// Gets or sets a reference to the current connection to the database.
        /// </summary>
        private MySqlDatabaseConnector Connector { get; set; }

        /// <summary>
        /// Gets or sets the SQL statement, table name or stored procedure to execute
        /// at the data source.
        /// </summary>
        /// <returns>
        /// The SQL statement or stored procedure to execute. The default is an
        /// empty string.
        /// </returns>
        public string CommandText { get => this.Command.CommandText; set => this.Command.CommandText = value; }

        /// <summary>
        /// Gets or sets a value indicating how the CommandText
        /// property is to be interpreted.
        /// </summary>
        /// <returns>
        ///  One of the <see cref="System.Data.CommandType" /> values. The default is Text.
        /// </returns>
        public CommandType CommandType { get => this.Command.CommandType; set => this.Command.CommandType = value; }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command
        /// and generating an error.
        /// </summary>
        /// <returns>
        /// The time in seconds to wait for the command to execute. The default is 30 seconds.
        /// </returns>
        public int CommandTimeout { get => this.Command.CommandTimeout; set => this.Command.CommandTimeout = value; }

        /// <summary>
        /// Gets the array of parameters of this command instance.
        /// </summary>
        /// <returns>
        /// The parameters of the SQL statement or stored procedure. The default
        /// is an empty collection.
        /// </returns>
        public IEnumerable<IDbDataParameter> Parameters => this.Command.Parameters;

        #endregion

        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDatabaseCommand"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="connector">The database connector.</param>
        /// <exception cref="System.ArgumentNullException">command can not be null.</exception>
        /// <exception cref="System.ArgumentNullException">connector can not be null.</exception>
        internal MySqlDatabaseCommand(MySqlCommand command, MySqlDatabaseConnector connector)
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command), $"{nameof(command)} can not be null.");
            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector), $"{nameof(connector)} can not be null.");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Command?.Dispose();
            this.Command = null;
            this.Connector = null;
        }

        /// <summary>
        /// Sends the CommandText to the <see cref="MySqlDatabaseConnector" />
        /// and builds a <see cref="MySqlDatabaseReader" />.
        /// </summary>
        /// <returns>
        /// A database reader object.
        /// </returns>
        public IDatabaseReader ExecuteReader()
        {
            this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
            this.Command.Prepare();
            return new MySqlDatabaseReader(this.Command.ExecuteReader());
        }

        /// <summary>
        /// Executes a SQL statement against the connection and returns the number
        /// of rows affected.
        /// </summary>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        public int ExecuteNonQuery()
        {
            this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
            return this.Command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the result set, or a null reference
        /// if the result set is empty.
        /// </returns>
        public object ExecuteScalar()
        {
            this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
            return this.Command.ExecuteScalar();
        }

        /// <summary>
        /// Adds an existing parameter to the command.
        /// </summary>
        /// <param name="parameter">Parameter to add.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(IDataParameter parameter)
        {
            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, Type type)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = DbTypeConverter.FromType(type)
            };

            if (type == typeof(Nullable<>))
                parameter.IsNullable = true;

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, Type type, long size)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = DbTypeConverter.FromType(type),
                Size = (int)size
            };

            if (type == typeof(Nullable<>))
                parameter.IsNullable = true;

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, Type type, byte precision, byte scale)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = DbTypeConverter.FromType(type),
                Precision = precision,
                Scale = scale
            };

            if (type == typeof(Nullable<>))
                parameter.IsNullable = true;

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, Type type, long size, byte precision, byte scale)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = DbTypeConverter.FromType(type),
                Size = (int)size,
                Precision = precision,
                Scale = scale
            };

            if (type == typeof(Nullable<>))
                parameter.IsNullable = true;

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <param name="value">Parameter value</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDataParameter AddParameter(string name, Type type, long size, byte precision, byte scale, object value)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = DbTypeConverter.FromType(type),
                Size = (int)size,
                Precision = precision,
                Scale = scale,
                Value = value ?? DBNull.Value
            };

            if (type == typeof(Nullable<>))
                parameter.IsNullable = true;

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, DbType type)
        {
            var parameter = this.Command.Parameters.AddWithValue(name, DBNull.Value);
            parameter.DbType = type;

            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, DbType type, bool isNullable)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = type,
                IsNullable = isNullable
            };

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, DbType type, long size)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = type,
                Size = (int)size
            };

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, DbType type, int size, bool isNullable)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = type,
                Size = size,
                IsNullable = isNullable
            };

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, DbType type, byte precision, byte scale)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = type,
                Precision = precision,
                Scale = scale
            };

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, DbType type, long size, byte precision, byte scale)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = type,
                Size = (int)size,
                Precision = precision,
                Scale = scale
            };

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, DbType type, byte precision, byte scale, bool isNullable)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = type,
                Precision = precision,
                Scale = scale,
                IsNullable = isNullable
            };

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <param name="isNullable">Indicates if the parameter is nullable.</param>
        /// <returns>
        /// The reference of the parameter recently added.
        /// </returns>
        public IDataParameter AddParameter(string name, DbType type, long size, byte precision, byte scale, bool isNullable)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = type,
                Size = (int)size,
                Precision = precision,
                Scale = scale,
                IsNullable = isNullable
            };

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        public IDataParameter AddParameter(string name, DbType type, long size, byte precision, byte scale, bool isNullable,
            object value)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                DbType = type,
                Size = (int)size,
                Precision = precision,
                Scale = scale,
                IsNullable = isNullable,
                Value = value ?? DBNull.Value
            };

            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Gets a parameter by index.
        /// </summary>
        /// <param name="index">Parameter index.</param>
        /// <returns>
        /// The reference of the parameter.
        /// </returns>
        public IDataParameter GetParameter(int index)
        {
            return this.Command.Parameters[index];
        }

        /// <summary>
        /// Gets a parameter by name.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <returns>
        /// The reference of the parameter.
        /// </returns>
        public IDataParameter GetParameter(string name)
        {
            return this.Command.Parameters[name];
        }

        /// <summary>
        /// Clears the parameters.
        /// </summary>
        public void ClearParameters()
        {
            this.Command.Parameters.Clear();
        }

        #endregion
    }
}