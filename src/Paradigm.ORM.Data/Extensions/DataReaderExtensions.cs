using System;
using System.Reflection;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IDatabaseReader"/> interface.
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <typeparam name="T">Expected type.</typeparam>
        /// <param name="reader">The database reader.</param>
        /// <param name="index">The column index.</param>
        /// <returns>The value of the column as <see cref="T"/></returns>
        public static object GetValue<T>(this IDatabaseReader reader, int index)
        {
            return reader.GetValue(index, typeof(T));
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <typeparam name="T">Expected type.</typeparam>
        /// <param name="reader">The database reader.</param>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column as <see cref="T"/></returns>
        public static object GetValue<T>(this IDatabaseReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name), typeof(T));
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="reader">The database reader.</param>
        /// <param name="name">The column name.</param>
        /// <returns>The column value as <see cref="object"/></returns>
        public static object GetValue(this IDatabaseReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="reader">The database reader.</param>
        /// <param name="property">A property info. The system will use the property name to search for the column.</param>
        /// <returns>The column value as <see cref="object"/></returns>
        public static object GetValue(this IDatabaseReader reader, PropertyInfo property)
        {
            return reader.GetValue(reader.GetOrdinal(property.Name), property.PropertyType);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="reader">The database reader.</param>
        /// <param name="name">The column name.</param>
        /// <param name="type">The expected type.</param>
        /// <returns>The column value as <see cref="object"/></returns>
        public static object GetValue(this IDatabaseReader reader, string name, Type type)
        {
            return reader.GetValue(reader.GetOrdinal(name), type);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="reader">The database reader.</param>
        /// <param name="index">The column index.</param>
        /// <param name="type">The expected type.</param>
        /// <returns>The column value as <see cref="object"/></returns>
        public static object GetValue(this IDatabaseReader reader, int index, Type type)
        {
            var value = reader.GetValue(index);

            if (value == null || value == DBNull.Value)
                return null;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return value.ToString() == "YES";

                case TypeCode.SByte:
                    return reader.GetSByte(index);

                case TypeCode.Int16:
                    return reader.GetInt16(index);

                case TypeCode.Int32:
                    return reader.GetInt32(index);

                case TypeCode.Int64:
                    return reader.GetInt64(index);

                case TypeCode.Byte:
                    return reader.GetByte(index);

                case TypeCode.UInt16:
                    return reader.GetUInt16(index);

                case TypeCode.UInt32:
                    return reader.GetUInt32(index);

                case TypeCode.UInt64:
                    return reader.GetUInt64(index);

                case TypeCode.Single:
                    return reader.GetFloat(index);

                case TypeCode.Double:
                    return reader.GetDouble(index);

                case TypeCode.Decimal:
                    return reader.GetDecimal(index);

                case TypeCode.DateTime:
                    return reader.GetDateTime(index);

                case TypeCode.String:
                        return reader.GetString(index);
            }

            if (type == typeof(Guid))
                return reader.GetGuid(index);

            if (type == typeof(TimeSpan))
                return reader.GetTimeSpan(index);

            return value;
        }
    }
}