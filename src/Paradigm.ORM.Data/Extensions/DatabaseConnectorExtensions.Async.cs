using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Querying;

namespace Paradigm.ORM.Data.Extensions
{
    public static partial class DatabaseConnectorExtensions
    {
        #region Public Methods

        /// <summary>
        /// Executes a query and return the results.
        /// </summary>
        /// <typeparam name="TResultType">The result type.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A list of <see cref="TResultType"/></returns>
        /// <seealso cref="Querying.Query{TResultType}"/>
        public static async Task<List<TResultType>> QueryAsync<TResultType>(this IDatabaseConnector connector, string whereClause = null, params object[] parameters) where TResultType : class
        {
            return await new Query<TResultType>(connector).ExecuteAsync(whereClause, parameters);
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
        public static async Task<List<TResultType>> CustomQueryAsync<TResultType>(this IDatabaseConnector connector, string query, string whereClause = null, params object[] parameters) where TResultType : class
        {
            return await new CustomQuery<TResultType>(connector, query).ExecuteAsync(whereClause, parameters);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static async Task ExecuteReaderAsync(this IDatabaseConnector connector, IDatabaseCommand command, Func<IDatabaseReader, Task> action)
        {
            using var reader = await command.ExecuteReaderAsync();
            await action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static async Task ExecuteReaderAsync(this IDatabaseConnector connector, IDatabaseCommand command, Action<IDatabaseReader> action)
        {
            using var reader = await command.ExecuteReaderAsync();
            action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static async Task<T> ExecuteReaderAsync<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<IDatabaseReader, T> action)
        {
            using var reader = await command.ExecuteReaderAsync();
            return action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static async Task ExecuteReaderAsync(this IDatabaseConnector connector, string commandText, Func<IDatabaseReader, Task> action)
        {
            using var command = connector.CreateCommand(commandText);
            using var reader = await command.ExecuteReaderAsync();
            await action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static async Task ExecuteReaderAsync(this IDatabaseConnector connector, string commandText, Action<IDatabaseReader> action)
        {
            using var command = connector.CreateCommand(commandText);
            using var reader = await command.ExecuteReaderAsync();
            action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static async Task<T> ExecuteReaderAsync<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<IDatabaseReader, Task<T>> action)
        {
            using var reader = await command.ExecuteReaderAsync();
            return await action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static async Task<T> ExecuteReaderAsync<T>(this IDatabaseConnector connector, string commandText, Func<IDatabaseReader, Task<T>> action)
        {
            using var command = connector.CreateCommand(commandText);
            using var reader = await command.ExecuteReaderAsync();
            return await action(reader);
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static async Task<T> ExecuteReaderAsync<T>(this IDatabaseConnector connector, string commandText, Func<IDatabaseReader, T> action)
        {
            using var command = connector.CreateCommand(commandText);
            using var reader = await command.ExecuteReaderAsync();
            return action(reader);
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        public static async Task ExecuteNonQueryAsync(this IDatabaseConnector connector, IDatabaseCommand command)
        {
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static async Task ExecuteNonQueryAsync(this IDatabaseConnector connector, IDatabaseCommand command, Func<int, Task> action)
        {
            await action(await command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static async Task ExecuteNonQueryAsync(this IDatabaseConnector connector, IDatabaseCommand command, Action<int> action)
        {
            action(await command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        public static async Task ExecuteNonQueryAsync(this IDatabaseConnector connector, string commandText)
        {
            using var command = connector.CreateCommand(commandText);
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static async Task ExecuteNonQueryAsync(this IDatabaseConnector connector, string commandText, Func<int, Task> action)
        {
            using var command = connector.CreateCommand(commandText);
            await action(await command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static async Task ExecuteNonQueryAsync(this IDatabaseConnector connector, string commandText, Action<int> action)
        {
            using var command = connector.CreateCommand(commandText);
            action(await command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static async Task<T> ExecuteNonQueryAsync<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<int, Task<T>> action)
        {
            return await action(await command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static async Task<T> ExecuteNonQueryAsync<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<int, T> action)
        {
            return action(await command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static async Task<T> ExecuteNonQueryAsync<T>(this IDatabaseConnector connector, string commandText, Func<int, Task<T>> action)
        {
            using var command = connector.CreateCommand(commandText);
            return await action(await command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static async Task<T> ExecuteNonQueryAsync<T>(this IDatabaseConnector connector, string commandText, Func<int, T> action)
        {
            using var command = connector.CreateCommand(commandText);
            return action(await command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static async Task ExecuteScalarAsync(this IDatabaseConnector connector, IDatabaseCommand command, Func<object, Task> action)
        {
            await action(await command.ExecuteScalarAsync());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static async Task ExecuteScalarAsync(this IDatabaseConnector connector, IDatabaseCommand command, Action<object> action)
        {
            action(await command.ExecuteScalarAsync());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static async Task ExecuteScalarAsync(this IDatabaseConnector connector, string commandText, Func<object, Task> action)
        {
            using var command = connector.CreateCommand(commandText);
            await action(await command.ExecuteScalarAsync());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static async Task ExecuteScalarAsync(this IDatabaseConnector connector, string commandText, Action<object> action)
        {
            using var command = connector.CreateCommand(commandText);
            action(await command.ExecuteScalarAsync());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static async Task<T> ExecuteScalarAsync<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<object, Task<T>> action)
        {
            return await action(await command.ExecuteScalarAsync());
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static async Task<T> ExecuteScalarAsync<T>(this IDatabaseConnector connector, string commandText, Func<object, Task<T>> action)
        {
            using var command = connector.CreateCommand(commandText);
            return await action(await command.ExecuteScalarAsync());
        }

        #endregion
    }
}