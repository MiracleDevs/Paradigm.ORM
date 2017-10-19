using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cassandra.Data;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.Cassandra.CommandBuilders
{
    /// <summary>
    /// Provide base methods and functionality for all the command builders.
    /// </summary>
    public abstract class CqlCommandBuilderBase : CommandBuilderBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlCommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        protected CqlCommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the name of the table already scaped.
        /// </summary>
        /// <returns>The name of the table.</returns>
        protected override string GetTableName()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // overrides the base method to prevent the schema info to be rendered.
            // Cql database does not have 3 level entities, only keyspace (catalog) and column families (tables).
            ////////////////////////////////////////////////////////////////////////////////////////

            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.Descriptor.CatalogName))
                builder.AppendFormat("{0}.", this.FormatProvider.GetEscapedName(this.Descriptor.CatalogName));

            builder.Append(this.FormatProvider.GetEscapedName(this.Descriptor.TableName));

            return builder.ToString();
        }

        /// <summary>
        /// Gets a string by joining all the property names, separated by a comma or a provided separator.
        /// </summary>
        /// <param name="columns">Array of column property descriptors.</param>
        /// <param name="separator">String separator. Comma is the default character used if no other is provided.</param>
        /// <returns>A string with all the parameter names.</returns>
        /// <example>:param1,:param2,:param3,...,:paramN</example>
        protected override string GetDbParameterNames(IEnumerable<IColumnDescriptor> columns, string separator = ",")
        {
            return string.Join(separator, columns.Select(x => $":{x.ColumnName}"));
        }

        /// <summary>
        /// Gets a string by joining all the property name and values, separated by comma or a provided separator.
        /// </summary>
        /// <param name="columns">Array of column property descriptors.</param>
        /// <param name="separator">String separator. Comma is the default character used if no other is provided.</param>
        /// <example>:param1='value1',:param2='value2',:param3='value3',...,:paramN='value4'</example>
        protected override string GetDbParameterNamesAndValues(IEnumerable<IColumnDescriptor> columns, string separator = ",")
        {
            return string.Join(separator, columns.Select(x => $"{this.FormatProvider.GetEscapedName(x.ColumnName)}=:{x.ColumnName}"));
        }

        /// <summary>
        /// Populates the command parameters using the collection of columns.
        /// </summary>
        /// <param name="columns">Array of column property descriptors</param>
        protected override void PopulateParameters(IEnumerable<IColumnDescriptor> columns)
        {
            var typeConverter = this.Connector.GetDbStringTypeConverter();

            foreach (var column in columns)
            {
                this.Command.AddParameter(new CqlParameter($":{column.ColumnName}"));
            }
        }
        #endregion
    }
}