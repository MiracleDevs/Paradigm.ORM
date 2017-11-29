using System;
using System.Reflection;

namespace Paradigm.ORM.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="TypeInfo"/> class.
    /// </summary>
    public static class TypeInfoExtensions
    {
        /// <summary>
        /// Gets the default value of a given TypeInfo.
        /// </summary>
        /// <param name="typeInfo">The type information.</param>
        /// <returns>Default Value.</returns>
        public static object GetDefaultValue(this TypeInfo typeInfo)
        {
            return typeInfo.IsValueType ? Activator.CreateInstance(typeInfo.AsType()) : null;
        }
    }
}