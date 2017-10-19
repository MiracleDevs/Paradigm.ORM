using System;

namespace Paradigm.ORM.Data.Converters
{
    /// <summary>
    /// Provides methods to convert from database objects to .net types.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Converters.IValueConverter" />
    public abstract class ValueConverterBase: IValueConverter
    {
        /// <inheritdoc />
        /// <summary>
        /// Converts an object to a given .net type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The type to convert to.</param>
        /// <returns>
        /// A value of the specified type.
        /// </returns>
        /// <remarks>
        /// This method only works for native type codes.
        /// </remarks>
        public virtual object ConvertTo(object value, Type type)
        {
            return NativeTypeConverter.ConvertTo(value, type);
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts an object to a given .net type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>Converted value.</returns>
        public virtual T ConvertTo<T>(object value)
        {
            return (T) this.ConvertTo(value, typeof(T));
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts from a .net type to a database type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dbType">The type.</param>
        /// <returns>Converted value.</returns>
        public virtual object ConvertFrom(object value, string dbType)
        {
            return value ?? DBNull.Value;
        }
    }
}