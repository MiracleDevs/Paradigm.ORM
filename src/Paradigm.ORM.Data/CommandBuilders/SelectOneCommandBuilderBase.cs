using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql select one command builder objects.
    /// </summary>
    /// <seealso cref="CommandBuilderBase" />
    /// <seealso cref="ISelectOneCommandBuilder" />
    public abstract class SelectOneCommandBuilderBase : CommandBuilderBase, ISelectOneCommandBuilder
    {
        #region Columns

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
        /// Initializes a new instance of the <see cref="SelectOneCommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        protected SelectOneCommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the select one command ready to execute the select query.
        /// </summary>
        /// <param name="ids">The id values of the entity that will be selected from the database.</param>
        /// <returns>
        /// A select command already parametrized to execute.
        /// </returns>
        public IDatabaseCommand GetCommand(params object[] ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids), "You should at least provide one Id.");

            var primaryKeys = this.Descriptor.PrimaryKeyColumns;

            if (ids.Length != primaryKeys.Count)
                throw new ArgumentException($"The id count does not match the entity primary key count (the entity has {primaryKeys.Count} keys).", nameof(ids));

            var command = this.Connector.CreateCommand(this.CommandText);
            var valueConverter = this.Connector.GetValueConverter();
            var typeConverter = this.Connector.GetDbStringTypeConverter();

            for (var i = 0; i < primaryKeys.Count; i++)
            {
                var column = primaryKeys[i];

                command.AddParameter(
                    this.FormatProvider.GetParameterName(column.ColumnName),
                    typeConverter.GetType(column.DataType),
                    column.MaxSize,
                    column.Precision,
                    column.Scale,
                    valueConverter.ConvertFrom(ids[i], this.Descriptor.PrimaryKeyColumns[i].DataType));
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
            var builder = new StringBuilder();

            builder.AppendFormat("SELECT {0} FROM {1}", this.FormatProvider.GetColumnNames(this.GetSelectableColumns()), this.FormatProvider.GetTableName(this.Descriptor));

            if (this.Descriptor.PrimaryKeyColumns.Any())
                builder.Append(" WHERE ");

            foreach (var primaryKey in this.Descriptor.PrimaryKeyColumns)
                builder.AppendFormat("{0}={1} AND ", this.FormatProvider.GetEscapedName(primaryKey.ColumnName), this.FormatProvider.GetParameterName(primaryKey.ColumnName));

            this.CommandText = builder.Remove(builder.Length - 5, 5).ToString();
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