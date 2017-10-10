using System;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.PostgreSql.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for postgresql select command builder objects.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.CommandBuilderBase" />
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.ISelectCommandBuilder" />
    public class PostgreSqlSelectCommandBuilder : CommandBuilderBase, ISelectCommandBuilder
    {
        #region Columns

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        private string CommandText { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlSelectCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public PostgreSqlSelectCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the select command ready to execute the select query.
        /// </summary>
        /// <param name="whereClause">An optional where clause to add to the query. If the where contains parameters, they need to be named as @1 @2 @3 etc.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>
        /// A select command already parametrized to execute.
        /// </returns>
        public IDatabaseCommand GetCommand(string whereClause = null, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(whereClause))
            {
                this.Command.CommandText = this.CommandText;
                return this.Command;
            }

            whereClause = $"WHERE {whereClause}";
            this.Command.CommandText = $"{this.CommandText} {whereClause}";
            this.Command.ClearParameters();

            if (parameters != null)
            {
                for (var index = 0; index < parameters.Length; index++)
                {
                    var parameter = parameters[index];
                    var type = parameter == null ? typeof(object) : parameter.GetType();
                    var commandParameter = this.Command.AddParameter($"@{index + 1}", DbTypeConverter.FromType(type));
                    commandParameter.Value = parameter ?? DBNull.Value;
                }
            }

            return this.Command;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the command builder.
        /// </summary>
        private void Initialize()
        {
            this.CommandText = $"SELECT {this.GetPropertyNames()} FROM {this.GetTableName()}";
            this.Command = this.Connector.CreateCommand(this.CommandText);
        }

        #endregion
    }
}