using System;
using System.Reflection;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides an interface to describe the mapping relationship between a column and a property.
    /// </summary>
    public interface IColumnPropertyDescriptor: IColumnDescriptor
    {
        /// <summary>
        /// Gets the property type.
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// Gets the inner type of the property if the property is nullable; will be the same as <see cref="PropertyType" /> if Type is not nullable.
        /// </summary>
        Type NotNullablePropertyType { get; }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        string PropertyName { get; }
    }
}