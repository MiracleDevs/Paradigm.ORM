using System;
using Paradigm.ORM.Data.CommandBuilders;

namespace Paradigm.ORM.Data.PostgreSql.CommandBuilders
{
    /// <summary>
    /// Provides an implementation of a command format provider for PostgreSql databases.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.ICommandFormatProvider" />
    public class PostgreSqlCommandFormatProvider: ICommandFormatProvider
    {
        /// <summary>
        /// Gets the name of an object (table, view, column, etc) escaped with the proper characters.
        /// </summary>
        /// <param name="name">The name to scape.</param>
        /// <returns>
        /// Scaped name.
        /// </returns>
        public string GetEscapedName(string name)
        {
            return $"\"{name}\"";
        }

        /// <summary>
        /// Gets the column value already formatted with the proper characters.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="type">The type of the value.</param>
        /// <returns>
        /// Formatted value.
        /// </returns>
        public string GetColumnValue(object value, Type type)
        {
            if (value == null)
                return "NULL";

            var bytes = value as byte[];

            if (bytes != null)
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
                    return value is bool && (bool)value ? "1" : "0";

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
        public string GetColumnValue(object value, string dataType)
        {
            if (value == null)
                return "NULL";

            var bytes = value as byte[];

            if (bytes != null)
                value = Convert.ToBase64String(bytes);

            if (value is DateTime)
                value = ((DateTime)value).ToString("yyyy-MM-dd hh:mm:ss");

            switch (dataType.ToLower())
            {
                case "bigint":
                case "int8":
                case "bigserial":
                case "serial8":
                case "int":
                case "integer":
                case "smallint":
                case "serial4":
                case "float":
                case "double":
                case "double precision":
                case "money":
                case "decimal":
                case "numeric":
                case "real":
                    return value.ToString();

                case "boolean":
                case "bool":
                case "bit":
                    return value is bool && (bool)value ? "1" : "0";
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
        public string GetQuerySeparator()
        {
            return ";";
        }
    }
}