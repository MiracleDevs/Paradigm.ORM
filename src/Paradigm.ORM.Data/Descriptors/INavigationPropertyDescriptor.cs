using System;
using System.Collections.Generic;
using System.Reflection;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides an interface to describe the relationship between two table type descriptors.
    /// </summary>
    public interface INavigationPropertyDescriptor
    {
        /// <summary>
        /// Gets the name of the property decorated with the navigation information.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets the type of the property decorated with the navigation information.
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// Gets the property information of the property decorated with the navigation information.
        /// </summary>
        PropertyInfo PropertyInfo { get;  }

        /// <summary>
        /// Gets the table type descriptor for the source type.
        /// </summary>
        ITableTypeDescriptor FromDescriptor { get; }

        /// <summary>
        /// Gets the table type descriptor for the referenced type.
        /// </summary>
        ITableTypeDescriptor ToDescriptor { get; }

        /// <summary>
        /// Gets the navigation attributes.
        /// </summary>
        IReadOnlyCollection<NavigationAttribute> NavigationAttributes { get; }

        /// <summary>
        /// Gets the navigation key descriptors.
        /// </summary>
        IReadOnlyCollection<INavigationKeyDescriptor> NavigationKeyDescriptors { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is the agregate root on the navigation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is root agregate; otherwise, <c>false</c>.
        /// </value>
        bool IsAggregateRoot { get; }
    }
}