using System;
using System.Collections.Generic;
using System.Data;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Querying;
using System.Linq;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Extensions
{
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
            using (var query = new Query<TResultType>(connector))
            {
                return query.Execute(whereClause, parameters);
            }
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
            using (var customQuery = new CustomQuery<TResultType>(connector, query))
            {
                return customQuery.Execute(whereClause, parameters);
            }
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static void ExecuteReader(this IDatabaseConnector connector, IDatabaseCommand command, Action<IDatabaseReader> action)
        {
            connector.OpenMySqlConnectionWhenClose();

            using (var reader = command.ExecuteReader())
            {
                action(reader);
            }
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static void ExecuteReader(this IDatabaseConnector connector, string commandText, Action<IDatabaseReader> action)
        {
            connector.OpenMySqlConnectionWhenClose();

            using (var command = connector.CreateCommand(commandText))
            {
                using (var reader = command.ExecuteReader())
                {
                    action(reader);
                }
            }
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static T ExecuteReader<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<IDatabaseReader, T> action)
        {
            connector.OpenMySqlConnectionWhenClose();

            using (var reader = command.ExecuteReader())
            {
                return action(reader);
            }
        }

        /// <summary>
        /// Executes a database command as data reader.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="action">A callback action to process the database reader.</param>
        public static T ExecuteReader<T>(this IDatabaseConnector connector, string commandText, Func<IDatabaseReader, T> action)
        {
            connector.OpenMySqlConnectionWhenClose();

            using (var command = connector.CreateCommand(commandText))
            {
                using (var reader = command.ExecuteReader())
                {
                    return action(reader);
                }
            }
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        public static void ExecuteNonQuery(this IDatabaseConnector connector, IDatabaseCommand command)
        {
            connector.OpenMySqlConnectionWhenClose();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        public static void ExecuteNonQuery(this IDatabaseConnector connector, string commandText)
        {
            connector.OpenMySqlConnectionWhenClose();

            using (var command = connector.CreateCommand(commandText))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static void ExecuteNonQuery(this IDatabaseConnector connector, IDatabaseCommand command, Action<int> action)
        {
            connector.OpenMySqlConnectionWhenClose();
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
            connector.OpenMySqlConnectionWhenClose();

            using (var command = connector.CreateCommand(commandText))
            {
                action(command.ExecuteNonQuery());
            }
        }

        /// <summary>
        /// Executes a database command as non query.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the affected rows quantity.</param>
        public static T ExecuteNonQuery<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<int, T> action)
        {
            connector.OpenMySqlConnectionWhenClose();
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
            connector.OpenMySqlConnectionWhenClose();

            using (var command = connector.CreateCommand(commandText))
            {
                return action(command.ExecuteNonQuery());
            }
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static void ExecuteScalar(this IDatabaseConnector connector, IDatabaseCommand command, Action<object> action)
        {
            connector.OpenMySqlConnectionWhenClose();
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
            connector.OpenMySqlConnectionWhenClose();

            using (var command = connector.CreateCommand(commandText))
            {
                action(command.ExecuteScalar());
            }
        }

        /// <summary>
        /// Executes a database command as scalar.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="command">The database command.</param>
        /// <param name="action">A callback action to process the scalar value.</param>
        public static T ExecuteScalar<T>(this IDatabaseConnector connector, IDatabaseCommand command, Func<object, T> action)
        {
            connector.OpenMySqlConnectionWhenClose();
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
            connector.OpenMySqlConnectionWhenClose();

            using (var command = connector.CreateCommand(commandText))
            {
                return action(command.ExecuteScalar());
            }
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
        /// <typeparam name="TParameters">The type of a class containing the parameters.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="descriptor">The routine type descriptor.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand<TParameters>(this IDatabaseConnector connector, string commandText, IRoutineTypeDescriptor descriptor)
        {
            return connector.CreateCommand<TParameters>(commandText, CommandType.StoredProcedure, descriptor);
        }

        /// <summary>
        /// Creates a database command.
        /// </summary>
        /// <typeparam name="TParameters">The type of a class containing the parameters.</typeparam>
        /// <param name="connector">The databaseconnector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="descriptor">A routine type descriptor for the parameters.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand<TParameters>(this IDatabaseConnector connector, string commandText, CommandType commandType, IRoutineTypeDescriptor descriptor)
        {
            var command = connector.CreateCommand(commandText);

            command.CommandType = CommandType.StoredProcedure;
            PopulateCommandParameters(connector, command, descriptor, default(TParameters), null);

            return command;
        }

        /// <summary>
        /// Creates a database command.
        /// </summary>
        /// <typeparam name="TParameters">The type of a class containing the parameters.</typeparam>
        /// <param name="connector">The databaseconnector.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand<TParameters>(this IDatabaseConnector connector, string commandText)
        {
            return connector.CreateCommand(commandText, CommandType.Text, default(TParameters), null);
        }

        /// <summary>
        /// Creates a database command.
        /// </summary>
        /// <typeparam name="TParameters">The type of a class containing the parameters.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand<TParameters>(this IDatabaseConnector connector, string commandText, CommandType commandType)
        {
            return connector.CreateCommand(commandText, commandType, default(TParameters), null);
        }

        /// <summary>
        /// Creates a database command and populates the parameter values.
        /// </summary>
        /// <typeparam name="TParameters">The type of a class containing the parameters.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameter type instance.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand<TParameters>(this IDatabaseConnector connector, string commandText, TParameters parameters)
        {
            return connector.CreateCommand(commandText, CommandType.Text, parameters, null);
        }

        /// <summary>
        /// Creates a database command and populates the parameter values.
        /// </summary>
        /// <typeparam name="TParameters">The type of a class containing the parameters.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameter type instance.</param>
        /// <param name="ignoreProperties">A list with properties to ignore.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand<TParameters>(this IDatabaseConnector connector, string commandText, TParameters parameters, params string[] ignoreProperties)
        {
            return connector.CreateCommand(commandText, CommandType.Text, parameters, ignoreProperties);
        }

        /// <summary>
        /// Creates a database command and populates the parameter values.
        /// </summary>
        /// <typeparam name="TParameters">The type of a class containing the parameters.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameter type instance.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand<TParameters>(this IDatabaseConnector connector, string commandText, CommandType commandType, TParameters parameters)
        {
            return connector.CreateCommand(commandText, parameters, null);
        }

        /// <summary>
        /// Creates a database command and populates the parameter values.
        /// </summary>
        /// <typeparam name="TParameters">The type of a class containing the parameters.</typeparam>
        /// <param name="connector">The database connector.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameter type instance.</param>
        /// <param name="ignoreProperties">A list with properties to ignore.</param>
        /// <returns>A database command.</returns>
        public static IDatabaseCommand CreateCommand<TParameters>(this IDatabaseConnector connector, string commandText, CommandType commandType, TParameters parameters, params string[] ignoreProperties)
        {
            var command = connector.CreateCommand(commandText);
            PopulateCommandParameters(connector, command, parameters, ignoreProperties);
            return command;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Opens my SQL connection when its closed.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        private static void OpenMySqlConnectionWhenClose(this IDatabaseConnector connector)
        {
            if (!connector.IsOpen())
            {
                connector.Open();
            }
        }

        /// <summary>
        /// Populates the command parameters.
        /// </summary>
        /// <typeparam name="TParameters">The type of the parameters.</typeparam>
        /// <param name="connector">Reference to a database connector.</param>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ignoreProperties">The ignore properties.</param>
        private static void PopulateCommandParameters<TParameters>(IDatabaseConnector connector, IDatabaseCommand command, TParameters parameters, string[] ignoreProperties)
        {
            var descriptor = new RoutineTypeDescriptor(typeof(TParameters));

            PopulateCommandParameters(connector, command, descriptor, parameters, ignoreProperties);
        }

        /// <summary>
        /// Populates the command parameters.
        /// </summary>
        /// <typeparam name="TParameters">The type of the parameters.</typeparam>
        /// <param name="connector">Reference to a database connector.</param>
        /// <param name="command">The command.</param>
        /// <param name="descriptor">The routine type descriptor.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ignoreProperties">The ignore properties.</param>
        /// <exception cref="System.ArgumentNullException">descriptor - descriptor</exception>
        private static void PopulateCommandParameters<TParameters>(IDatabaseConnector connector, IDatabaseCommand command, IRoutineTypeDescriptor descriptor, TParameters parameters, string[] ignoreProperties)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor), $"{nameof(descriptor)} can not be null.");

            var formatProvider = connector.GetCommandFormatProvider();

            foreach (var parameter in descriptor.Parameters)
            {
                if (ignoreProperties != null && ignoreProperties.Contains(parameter.ParameterName))
                    continue;

                var commandParameter = command.AddParameter(formatProvider.GetParameterName(parameter.ParameterName), DbTypeConverter.FromType(parameter.Type));
                commandParameter.Direction = parameter.IsInput ? ParameterDirection.Input : ParameterDirection.Output;

                if (parameters == null || parameters.Equals(default(TParameters)))
                    continue;

                var value = parameter.PropertyInfo.GetValue(parameters);
                commandParameter.Value = value ?? DBNull.Value;
            }
        }

        #endregion
    }
}