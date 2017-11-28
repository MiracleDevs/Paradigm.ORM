using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Descriptors;

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
        /// Gets the name of the parameter already formatted for ado.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A formatted representation of the name.</returns>
        string GetParameterName(string name);

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

        /// <summary>
        /// Gets the name of the table already escaped.
        /// </summary>
        /// <param name="descriptor">A reference to a table descriptor.</param>
        /// <returns>An escaped table name.</returns>
        string GetTableName(ITableDescriptor descriptor);

        /// <summary>
        /// Gets the name of the routine already escaped.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>An escaped routine name.</returns>
        string GetRoutineName(IRoutineTypeDescriptor descriptor);

        /// <summary>
        /// Gets all the column names separated wht commas and escaped.
        /// </summary>
        /// <param name="descriptors">Array of column property descriptors.</param>
        /// <param name="separator">String separator. Comma is the default character used if no other is provided.</param>
        /// <returns>All the property names.</returns>
        string GetColumnNames(IEnumerable<IColumnDescriptor> descriptors, string separator = ",");

        /// <summary>
        /// Gets a string by joining all the parameter names, separated by a comma or a provided separator.
        /// </summary>
        /// <param name="columns">Array of column descriptors.</param>
        /// <param name="separator">String separator. Comma is the default character used if no other is provided.</param>
        /// <returns>A string with all the parameter names.</returns>
        /// <example>@param1,@param2,@param3,...,@paramN</example>
        string GetDbParameterNames(IEnumerable<IColumnDescriptor> columns, string separator = ",");

        /// <summary>
        /// Gets a string by joining all the parameter name and values, separated by comma or a provided separator.
        /// </summary>
        /// <param name="columns">Array of column descriptors.</param>
        /// <param name="separator">String separator. Comma is the default character used if no other is provided.</param>
        /// <example>@param1='value1',@param2='value2',@param3='value3',...,@paramN='value4'</example>
        string GetDbParameterNamesAndValues(IEnumerable<IColumnDescriptor> columns, string separator = ",");
    }
}