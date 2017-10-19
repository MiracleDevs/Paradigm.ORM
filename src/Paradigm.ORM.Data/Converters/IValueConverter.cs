using System;

namespace Paradigm.ORM.Data.Converters
{
    /// <summary>
    /// Provides methods to convert a <see cref="Object"/> to other native types.
    /// </summary>
    public interface IValueConverter
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
        object ConvertTo(object value, Type type);

        /// <summary>
        /// Converts an object to a given .net type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>Converted value.</returns>
        T ConvertTo<T>(object value);

        /// <summary>
        /// Converts from a .net type to a database type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dbType">The type.</param>
        /// <returns>Converted value.</returns>
        object ConvertFrom(object value, string dbType);
    }
}