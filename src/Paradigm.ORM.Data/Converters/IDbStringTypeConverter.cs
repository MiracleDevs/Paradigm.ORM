using System;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Converters
{
    /// <summary>
    /// Provides an interface to convert from database types or database schema tables to .net types.
    /// </summary>
    public interface IDbStringTypeConverter
    {
        /// <summary>
        /// Gets the equivalent .net type from a database type.
        /// </summary>
        /// <param name="dbType">A database type.</param>
        /// <param name="isNullable">True if the value is nullable; false otherwise.</param>
        /// <returns>The equivalent .net type.</returns>
        Type GetType(string dbType, bool isNullable = false);

        /// <summary>
        /// Gets the equivalent .net type from a parameter schema.
        /// </summary>
        /// <param name="parameter">The parameter schema.</param>
        /// <returns>The equivalent .net type.</returns>
        Type GetType(IParameter parameter);

        /// <summary>
        /// Gets the equivalent .net type from a column schema.
        /// </summary>
        /// <param name="column">The column schema.</param>
        /// <returns>The equivalent .net type.</returns>
        Type GetType(IColumn column);

        /// <summary>
        /// Gets the equivalent database type from a .net type.
        /// </summary>
        /// <param name="type">The .net type.</param>
        /// <returns>The equivalent database type.</returns>
        string GetDbStringType(Type type);
    }
}