using System;
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
                    switch (value)
                    {
                        case LocalDate localDate:
                            return new DateTime(localDate.Year, localDate.Month, localDate.Day);

                        case DateTimeOffset offset:
                            return offset.DateTime;
                    }
                    throw new NotSupportedException();

                case TypeCode.Boolean:
                    return Convert.ToBoolean(value);

                case TypeCode.String:
                    return Convert.ToString(value);
            }

            if (type == typeof(TimeSpan))
            {
                if (value is LocalTime localTime)
                    return new TimeSpan(localTime.Hour, localTime.Minute, localTime.Second);

                throw new NotSupportedException();
            }

            if (type == typeof(Guid))
            {
                return (Guid)value;
            }

            return value;
        }
    }
}