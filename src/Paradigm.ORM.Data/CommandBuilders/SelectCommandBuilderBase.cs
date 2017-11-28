using System;
using System.Collections.Generic;
using System.Text;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql select command builder objects.
    /// </summary>
    /// <seealso cref="CommandBuilderBase" />
    /// <seealso cref="ISelectCommandBuilder" />
    public abstract class SelectCommandBuilderBase : CommandBuilderBase, ISelectCommandBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        protected string CommandText { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        protected SelectCommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
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
            if (string.IsNullOrEmpty(whereClause) && parameters != null && parameters.Length > 0)
                throw new Exception("Can not pass parameters without a where clause.");

            var builder = new StringBuilder(this.CommandText);

            if (!string.IsNullOrWhiteSpace(whereClause))
                builder.AppendFormat("WHERE {0}", whereClause);

            var command = this.Connector.CreateCommand(this.CommandText);

            if (parameters == null)
                return command;

            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                var name = this.FormatProvider.GetParameterName((index + 1).ToString());
                var type = parameter == null ? typeof(object) : parameter.GetType();
                var commandParameter = command.AddParameter(name, DbTypeConverter.FromType(type));
                commandParameter.Value = parameter;
            }

            return command;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the command builder.
        /// </summary>
        private void Initialize()
        {
            this.CommandText = $"SELECT {this.FormatProvider.GetColumnNames(this.GetSelectableColumns())} FROM {this.FormatProvider.GetTableName(this.Descriptor)}";
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a list of column descriptors that must be included in the select statement.
        /// </summary>
        /// <returns>A list of column descriptors.</returns>
        private List<IColumnDescriptor> GetSelectableColumns()
        {
            return this.Descriptor.AllColumns;
        }

        #endregion
    }
}