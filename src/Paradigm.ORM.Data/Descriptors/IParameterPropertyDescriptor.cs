using System;
using System.Reflection;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides an interface to describe the mapping relationship between a parameter and a property.
    /// </summary>
    public interface IParameterPropertyDescriptor
    {
        /// <summary>
        /// Gets the property type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the inner type of the property if the property is nullable; will be the same as <see cref="Type" /> if Type is not nullable.
        /// </summary>
        Type NotNullableType { get; }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        string ParameterName { get; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets the maximum size.
        /// </summary>
        long MaxSize { get; }

        /// <summary>
        /// Gets the numeric precision.
        /// </summary>
        byte Precision { get; }

        /// <summary>
        /// Gets the numeric scale.
        /// </summary>
        byte Scale { get; }

        /// <summary>
        /// Gets a value indicating whether this property is input or output property.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this property is an input property; otherwise, <c>false</c>.
        /// </value>
        bool IsInput { get; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        string DataType { get; }
    }
}