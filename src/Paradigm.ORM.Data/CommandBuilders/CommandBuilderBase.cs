using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.Descriptors;
using System.Text;
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
        protected IDatabaseConnector Connector { get; private set; }

        /// <summary>
        /// Gets the command format provider related to the current connector.
        /// </summary>
        protected ICommandFormatProvider FormatProvider { get; private set; }

        /// <summary>
        /// Gets the table descriptor related with the command builder.
        /// </summary>
        /// <remarks>
        /// Command builders are related and constructed arround a given type,
        /// and the ORM uses <see cref="TableDescriptor" /> to gain contextual
        /// information about the table.
        /// </remarks>
        protected ITableDescriptor Descriptor { get; private set; }

        /// <summary>
        /// Gets the command instance.
        /// </summary>
        protected IDatabaseCommand Command { get; set; }

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

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Command?.Dispose();
            this.FormatProvider = null;
            this.Connector = null;
            this.Command = null;
            this.Descriptor = null;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the name of the table already scaped.
        /// </summary>
        /// <returns>The name of the table.</returns>
        protected virtual string GetTableName()
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.Descriptor.CatalogName))
                builder.AppendFormat("{0}.", this.FormatProvider.GetEscapedName(this.Descriptor.CatalogName));

            if (!string.IsNullOrWhiteSpace(this.Descriptor.SchemaName))
                builder.AppendFormat("{0}.", this.FormatProvider.GetEscapedName(this.Descriptor.SchemaName));

            builder.Append(this.FormatProvider.GetEscapedName(this.Descriptor.TableName));

            return builder.ToString();
        }

        /// <summary>
        /// Gets all the property names separated wht commas and scaped.
        /// </summary>
        /// <returns>All the property names.</returns>
        protected virtual string GetPropertyNames()
        {
            return this.GetPropertyNames(this.Descriptor.AllColumns);
        }

        /// <summary>
        /// Gets all the property names separated wht commas and scaped.
        /// </summary>
        /// <param name="descriptors">Array of column property descriptors.</param>
        /// <returns>All the property names.</returns>
        protected virtual string GetPropertyNames(IEnumerable<IColumnDescriptor> descriptors)
        {
            return string.Join(",", descriptors.Select(x => this.FormatProvider.GetEscapedName(x.ColumnName)));
        }

        /// <summary>
        /// Gets a string by joining all the property names, separated by a comma or a provided separator.
        /// </summary>
        /// <param name="columns">Array of column property descriptors.</param>
        /// <param name="separator">String separator. Comma is the default character used if no other is provided.</param>
        /// <returns>A string with all the parameter names.</returns>
        /// <example>@param1,@param2,@param3,...,@paramN</example>
        protected virtual string GetDbParameterNames(IEnumerable<IColumnDescriptor> columns, string separator = ",")
        {
            return string.Join(separator, columns.Select(x => $"@{x.ColumnName}"));
        }

        /// <summary>
        /// Gets a string by joining all the property name and values, separated by comma or a provided separator.
        /// </summary>
        /// <param name="columns">Array of column property descriptors.</param>
        /// <param name="separator">String separator. Comma is the default character used if no other is provided.</param>
        /// <example>@param1='value1',@param2='value2',@param3='value3',...,@paramN='value4'</example>
        protected virtual string GetDbParameterNamesAndValues(IEnumerable<IColumnDescriptor> columns, string separator = ",")
        {
            return string.Join(separator, columns.Select(x => $"{this.FormatProvider.GetEscapedName(x.ColumnName)}=@{x.ColumnName}"));
        }

        /// <summary>
        /// Populates the command parameters using the collection of columns of the table type descriptor.
        /// </summary>
        protected virtual void PopulateParameters()
        {
            this.PopulateParameters(this.Descriptor.AllColumns);
        }

        /// <summary>
        /// Populates the command parameters using the collection of columns.
        /// </summary>
        /// <param name="columns">Array of column property descriptors</param>
        protected virtual void PopulateParameters(IEnumerable<IColumnDescriptor> columns)
        {
            var typeConverter = this.Connector.GetDbStringTypeConverter();

            foreach (var column in columns)
            {
                this.Command.AddParameter($"@{column.ColumnName}", typeConverter.GetType(column.DataType), column.MaxSize, column.Precision, column.Scale);
            }
        }

        #endregion
    }
}