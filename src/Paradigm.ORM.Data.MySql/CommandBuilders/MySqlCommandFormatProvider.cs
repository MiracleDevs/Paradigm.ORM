using System;
using System.Text;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.MySql.CommandBuilders
{
    /// <summary>
    /// Provides an implementation of a command format provider for MySql databases.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.ICommandFormatProvider" />
    public class MySqlCommandFormatProvider : CommandFormatProviderBase
    {
        /// <summary>
        /// Gets the name of an object (table, view, column, etc) escaped with the proper characters.
        /// </summary>
        /// <param name="name">The name to scape.</param>
        /// <returns>
        /// Scaped name.
        /// </returns>
        public override string GetEscapedName(string name)
        {
            return $"`{name}`";
        }

        /// <summary>
        /// Gets the name of the parameter already formatted for ado.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// A formatted representation of the name.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string GetParameterName(string name)
        {
            return $"@{name}";
        }

        /// <summary>
        /// Gets the column value already formatted with the proper characters.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="type">The type of the value.</param>
        /// <returns>
        /// Formatted value.
        /// </returns>
        public override string GetColumnValue(object value, Type type)
        {
            if (value == null)
                return "NULL";

            if (value is byte[] bytes)
                value = Convert.ToBase64String(bytes);

            if (value is DateTime)
                value = ((DateTime)value).ToString("yyyy-MM-dd hh:mm:ss");   

            if (type == typeof(Nullable<>))
                type = type.GenericTypeArguments[0];

            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return value.ToString();

                case TypeCode.Boolean:
                    return value is bool b && b ? "1" : "0";

                default:
                    return $"'{value}'";
            }
        }

        /// <summary>
        /// Gets the column value already formatted with the proper characters.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="dataType">The type of the value.</param>
        /// <returns>Formatted value.</returns>
        public override string GetColumnValue(object value, string dataType)
        {
            if (value == null)
                return "NULL";

            if (value is byte[] bytes)
                value = Convert.ToBase64String(bytes);

            if (value is DateTime)
                value = ((DateTime)value).ToString("yyyy-MM-dd hh:mm:ss");

            switch (dataType.ToLower())
            {
                case "boolean":
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "int":
                case "bigint":
                case "float":
                case "double":
                case "decimal":
                case "year":
                    return value.ToString();

                default:
                    return $"'{value}'";
            }
        }

        /// <summary>
        /// Gets the query separator.
        /// </summary>
        /// <returns>
        /// The database query separator, normally ';'.
        /// </returns>
        public override string GetQuerySeparator()
        {
            return ";";
        }

        /// <summary>
        /// Gets the name of the table already escaped.
        /// </summary>
        /// <param name="descriptor">A reference to a table descriptor.</param>
        /// <returns>
        /// An escaped table name.
        /// </returns>
        public override string GetTableName(ITableDescriptor descriptor)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // overrides the base method to prevent the schema info to be rendered.
            // MySql database does not have 3 level entities, only schema (catalog) and tables.
            ////////////////////////////////////////////////////////////////////////////////////////
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(descriptor.CatalogName))
                builder.AppendFormat("{0}.", this.GetEscapedName(descriptor.CatalogName));

            builder.Append(this.GetEscapedName(descriptor.TableName));

            return builder.ToString();
        }

        /// <summary>
        /// Gets the name of the routine already escaped.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>
        /// An escaped routine name.
        /// </returns>
        public override string GetRoutineName(IRoutineTypeDescriptor descriptor)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // overrides the base method to prevent the schema info to be rendered.
            // MySql database does not have 3 level entities, only schema (catalog) and routines.
            ////////////////////////////////////////////////////////////////////////////////////////
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(descriptor.CatalogName))
                builder.AppendFormat("{0}.", this.GetEscapedName(descriptor.CatalogName));

            builder.Append(this.GetEscapedName(descriptor.RoutineName));

            return builder.ToString();
        }
    }
}