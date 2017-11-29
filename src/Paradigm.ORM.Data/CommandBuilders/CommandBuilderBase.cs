using System.Collections.Generic;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides base and common functionality for all kind of command builders.
    /// </summary>
    public abstract class CommandBuilderBase : ICommandBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the current database connector.
        /// </summary>
        protected IDatabaseConnector Connector { get; }

        /// <summary>
        /// Gets the command format provider related to the current connector.
        /// </summary>
        protected ICommandFormatProvider FormatProvider { get; }

        /// <summary>
        /// Gets the table descriptor related with the command builder.
        /// </summary>
        /// <remarks>
        /// Command builders are related and constructed arround a given type,
        /// and the ORM uses <see cref="TableDescriptor" /> to gain contextual
        /// information about the table.
        /// </remarks>
        protected ITableDescriptor Descriptor { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        protected CommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor)
        {
            this.Connector = connector;
            this.Descriptor = descriptor;
            this.FormatProvider = connector.GetCommandFormatProvider();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Populates the command parameters using the collection of columns of the table type descriptor.
        /// </summary>
        /// <param name="command">A refecence to a database command.</param>
        protected virtual void PopulateParameters(IDatabaseCommand command)
        {
            this.PopulateParameters(command, this.Descriptor.AllColumns);
        }

        /// <summary>
        /// Populates the command parameters using the collection of columns.
        /// </summary>
        /// <param name="command">A refecence to a database command.</param>
        /// <param name="columns">Array of column property descriptors</param>
        protected virtual void PopulateParameters(IDatabaseCommand command, IEnumerable<IColumnDescriptor> columns)
        {
            var typeConverter = this.Connector.GetDbStringTypeConverter();

            foreach (var column in columns)
            {
                command.AddParameter($"{this.FormatProvider.GetParameterName(column.ColumnName)}", typeConverter.GetType(column.DataType), column.MaxSize, column.Precision, column.Scale);
            }
        }

        #endregion
    }
}