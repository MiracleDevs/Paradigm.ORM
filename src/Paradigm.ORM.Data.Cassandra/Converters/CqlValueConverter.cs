﻿using System;
using Cassandra;
using Paradigm.ORM.Data.Converters;

namespace Paradigm.ORM.Data.Cassandra.Converters
{
    /// <inheritdoc />
    /// <summary>
    /// Provides methods to convert from database objects to .net types.
    /// </summary>
    /// <seealso cref="T:Paradigm.ORM.Data.Converters.IValueConverter" />
    /// <seealso cref="T:Paradigm.ORM.Data.Converters.ValueConverterBase" />
    public class CqlValueConverter : ValueConverterBase
    {
        /// <inheritdoc />
        /// <summary>
        /// Converts an object to a given .net type.
        /// </summary>
        /// <remarks>
        /// This method only works for native type codes.
        /// </remarks>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The type to convert to.</param>
        /// <returns>A value of the specified type.</returns>
        public override object ConvertTo(object value, Type type)
        {
            if (value == DBNull.Value || value == null)
                return null;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Char:
                    return Convert.ToChar(value);

                case TypeCode.Byte:
                    return Convert.ToByte(value);

                case TypeCode.SByte:
                    return Convert.ToSByte(value);

                case TypeCode.Int16:
                    return Convert.ToInt16(value);

                case TypeCode.UInt16:
                    return Convert.ToInt16(value);

                case TypeCode.Int32:
                    return Convert.ToInt32(value);

                case TypeCode.UInt32:
                    return Convert.ToUInt32(value);

                case TypeCode.Int64:
                    return Convert.ToInt64(value);

                case TypeCode.UInt64:
                    return Convert.ToUInt64(value);

                case TypeCode.Single:
                    return Convert.ToSingle(value);

                case TypeCode.Double:
                    return Convert.ToDouble(value);

                case TypeCode.Decimal:
                    return Convert.ToDecimal(value);

                case TypeCode.DateTime:
                    return value switch
                    {
                        LocalDate localDate => new DateTime(localDate.Year, localDate.Month, localDate.Day),
                        DateTimeOffset offset => offset.DateTime,
                        _ => throw new NotSupportedException()
                    };

                case TypeCode.Boolean:
                    return Convert.ToBoolean(value);

                case TypeCode.String:
                    return Convert.ToString(value);
            }

            if (type == typeof(TimeSpan))
            {
                if (value is LocalTime localTime)
                    return new TimeSpan(0, localTime.Hour, localTime.Minute, localTime.Second, localTime.Nanoseconds / 1000000);

                throw new NotSupportedException();
            }

            return value;
        }

        /// <summary>
        /// Converts from a .net type to a database type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dbType">The type.</param>
        /// <returns>
        /// Converted value.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        /// <inheritdoc />
        public override object ConvertFrom(object value, string dbType)
        {
            if (value == DBNull.Value || value == null)
                return null;

            switch (dbType.ToLower())
            {
                case "date":
                    if (value is DateTime date)
                        return new LocalDate(date.Year, date.Month, date.Day);

                    return value;

                case "time":
                    if (value is TimeSpan time)
                        return new LocalTime(time.Hours, time.Minutes, time.Seconds, time.Milliseconds * 1000000);

                    return value;

                case "timestamp":
                    if (value is DateTime dateTime)
                        return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, TimeSpan.Zero);

                    return value;

                default:
                    return value;
            }
        }
    }
}