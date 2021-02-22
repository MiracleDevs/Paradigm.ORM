using System;
using System.Collections.Generic;
using System.Data;
using Paradigm.ORM.Data.Querying;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IDatabaseConnector"/> interface.
    /// </summary>
    public static partial class DatabaseConnectorExtensions
    {
        #region public static Methods

        /// <summary>
        /// Executes a query and return the results.
        /// </summary>
        /// <typeparam name="TResultType">The result type.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A list of <see cref="TResultType"/></returns>
        /// <seealso cref="Querying.Query{TResultType}"/>
        public static List<TResultType> Query<TResultType>(this IDatabaseConnector connector, string whereClause = null, params object[] parameters) where TResultType : class
        {
            return new Query<TResultType>(connector).Execute(whereClause, parameters);
        }

        /// <summary>
        /// Executes a query and return the results.
        /// </summary>
        /// <typeparam name="TResultType">The result type.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="query">A custom commandText string. The select columns should be mapped to the <see cref="TResultType"/></param>
        /// /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A list of <see cref="TResultType"/></returns>
        /// <seealso cref="Querying.CustomQuery{TResultType}"/>
        public static List<TResultType> CustomQuery<TResultType>(this IDatabaseConnector connector, string query, string whereClause = null, params object[] parameters) where TResultType : class
        {
            return new CustomQuery<TResultType>(connector, query).Execute(whereClause, parameters);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static void ExecuteReader(this IDatabaseConnector connector, IDatabaseCommand command, Action<IDatabaseReader> action)
        {
            using var reader = command.ExecuteReader();
            action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static void ExecuteReader(this IDatabaseConnector connector, string commandText, Action<IDatabaseReader> action)
        {
            using var command = connector.CreateCommand(commandText);
            using var reader = command.ExecuteReader();
            action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static T ExecuteReader<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<IDatabaseReader, T> action)
        {
            using var reader = command.ExecuteReader();
            return action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static T ExecuteReader<T>(this IDatabaseConnector connector, string commandText, Func<IDatabaseReader, T> action)
        {
            using var command = connector.CreateCommand(commandText);
            using var reader = command.ExecuteReader();
            return action(reader);
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        public static void ExecuteNonQuery(this IDatabaseConnector connector, IDatabaseCommand command)
        {
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        public static void ExecuteNonQuery(this IDatabaseConnector connector, string commandText)
        {
            using var command = connector.CreateCommand(commandText);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static void ExecuteNonQuery(this IDatabaseConnector connector, IDatabaseCommand command, Action<int> action)
        {
            action(command.ExecuteNonQuery());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static void ExecuteNonQuery(this IDatabaseConnector connector, string commandText, Action<int> action)
        {
            using var command = connector.CreateCommand(commandText);
            action(command.ExecuteNonQuery());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static T ExecuteNonQuery<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<int, T> action)
        {
            return action(command.ExecuteNonQuery());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static T ExecuteNonQuery<T>(this IDatabaseConnector connector, string commandText, Func<int, T> action)
        {
            using var command = connector.CreateCommand(commandText);
            return action(command.ExecuteNonQuery());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static void ExecuteScalar(this IDatabaseConnector connector, IDatabaseCommand command, Action<object> action)
        {
            action(command.ExecuteScalar());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static void ExecuteScalar(this IDatabaseConnector connector, string commandText, Action<object> action)
        {
            using var command = connector.CreateCommand(commandText);
            action(command.ExecuteScalar());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static T ExecuteScalar<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<object, T> action)
        {
            return action(command.ExecuteScalar());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static T ExecuteScalar<T>(this IDatabaseConnector connector, string commandText, Func<object, T> action)
        {
            using var command = connector.CreateCommand(commandText);
            return action(command.ExecuteScalar());
        }

        /// <summary>
        /// Creates a database command.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand(this IDatabaseConnector connector, string commandText)
        {
            var command = connector.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// Creates a database command.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand(this IDatabaseConnector connector, string commandText, CommandType commandType)
        {
            var command = connector.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        #endregion
    }
}