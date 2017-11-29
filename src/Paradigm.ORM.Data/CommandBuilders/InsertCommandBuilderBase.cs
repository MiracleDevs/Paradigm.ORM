using System.Collections.Generic;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql insert command builder objects.
    /// </summary>
    /// <seealso cref="CommandBuilderBase" />
    /// <seealso cref="IInsertCommandBuilder" />
    public abstract class InsertCommandBuilderBase : CommandBuilderBase, IInsertCommandBuilder
    {
        #region Properties

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        private string CommandText { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        protected InsertCommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an insert command query ready to insert one entity.
        /// </summary>
        /// <param name="valueProvider"></param>
        /// <returns>
        /// An insert command already parametrized to insert the entity.
        /// </returns>
        public IDatabaseCommand GetCommand(IValueProvider valueProvider)
        {
            var typeConverter = this.Connector.GetDbStringTypeConverter();
            var command = this.Connector.CreateCommand(this.CommandText);

            foreach (var column in this.GetPopulableColumns())
            {
                command.AddParameter(
                    this.FormatProvider.GetParameterName(column.ColumnName),
                    typeConverter.GetType(column.DataType),
                    column.MaxSize,
                    column.Precision,
                    column.Scale,
                    valueProvider.GetValue(column));
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
            var columns = this.GetInsertColumns();
            this.CommandText = $"INSERT INTO {this.FormatProvider.GetTableName(this.Descriptor)} ({this.FormatProvider.GetColumnNames(columns)}) VALUES ({this.FormatProvider.GetDbParameterNames(columns)})";
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a list of column descriptors that must be used in the insert statement.
        /// </summary>
        /// <remarks>
        /// Some databases may impose restrictions or limitation to the columns that can be
        /// inserted due to type or other rules. For ejample, TIMESTAMP type in sql server
        /// can not be inserted nor updated.
        /// </remarks>
        /// <returns>A list of column descriptors.</returns>
        protected virtual List<IColumnDescriptor> GetInsertColumns()
        {
            return this.Descriptor.SimpleColumns;
        }

        /// <summary>
        /// Gets a list of column descriptors used to populate the command parameters.
        /// </summary>
        /// <remarks>
        /// Some databases may impose restrictions or limitation to the columns that can be
        /// inserted due to type or other rules. For ejample, TIMESTAMP type in sql server
        /// can not be inserted nor updated.
        /// </remarks>
        /// <returns>A list of column descriptors.</returns>
        protected virtual List<IColumnDescriptor> GetPopulableColumns()
        {
            return this.GetInsertColumns();
        }

        #endregion
    }
}