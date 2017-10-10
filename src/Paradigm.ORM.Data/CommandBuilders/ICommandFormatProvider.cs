using System;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface for a command format provider.
    /// </summary>
    /// <remarks>
    /// The command format provider provides regular query formatting functions,
    /// that may change between different database implementations.
    /// </remarks>
    public interface ICommandFormatProvider
    {
        /// <summary>
        /// Gets the name of an object (table, view, column, etc) escaped with the proper characters.
        /// </summary>
        /// <param name="name">The name to scape.</param>
        /// <returns>Scaped name.</returns>
        string GetEscapedName(string name);

        /// <summary>
        /// Gets the column value already formatted with the proper characters.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="type">The type of the value.</param>
        /// <returns>Formatted value.</returns>
        string GetColumnValue(object value, Type type);

        /// <summary>
        /// Gets the column value already formatted with the proper characters.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="dataType">The type of the value.</param>
        /// <returns>Formatted value.</returns>
        string GetColumnValue(object value, string dataType);

        /// <summary>
        /// Gets the query separator.
        /// </summary>
        /// <returns>The database query separator, normally ';'.</returns>
        string GetQuerySeparator();
    }
}