using System;

namespace Paradigm.ORM.Data.Converters
{
    /// <summary>
    /// Provides methods to convert a <see cref="Object"/> to other native types.
    /// </summary>
    public static class NativeTypeConverter
    {
        /// <summary>
        /// Converts an object to a given .net type.
        /// </summary>
        /// <remarks>
        /// This method only works for native type codes.
        /// </remarks>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The type to convert to.</param>
        /// <returns>A value of the specified type.</returns>
        public static object ConvertTo(object value, Type type)
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
                    return Convert.ToDateTime(value);

                case TypeCode.Boolean:
                    var strValue = value.ToString();

                    switch (strValue.ToLower())
                    {
                        case "1":
                        case "true":
                        case "yes":
                            return true;

                        case "0":
                        case "false":
                        case "no":
                            return false;
                    }

                    return Convert.ToBoolean(value);

                case TypeCode.String:
                    return Convert.ToString(value);

                default:
                    return value;
            }
        }
    }
}