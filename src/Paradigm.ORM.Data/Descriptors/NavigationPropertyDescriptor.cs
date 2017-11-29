using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides the means to describe the navigation relationship between two types.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Descriptors.INavigationPropertyDescriptor" />
    internal class NavigationPropertyDescriptor: INavigationPropertyDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the property decoration.
        /// </summary>
        private PropertyDecoration PropertyDecoration { get; }

        /// <summary>
        /// Gets the name of the property decorated with the navigation information.
        /// </summary>
        public virtual string PropertyName { get; private set; }

        /// <summary>
        /// Gets the type of the property decorated with the navigation information.
        /// </summary>
        public virtual Type PropertyType { get; private set; }

        /// <summary>
        /// Gets the property information of the property decorated with the navigation information.
        /// </summary>
        public virtual PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the table type descriptor for the source type.
        /// </summary>
        public virtual ITableTypeDescriptor FromDescriptor { get; }

        /// <summary>
        /// Gets the table type descriptor for the referenced type.
        /// </summary>
        public virtual ITableTypeDescriptor ToDescriptor { get; private set; }

        /// <summary>
        /// Gets the navigation attributes.
        /// </summary>
        public virtual IReadOnlyCollection<NavigationAttribute> NavigationAttributes { get; private set; }

        /// <summary>
        /// Gets the navigation key descriptors.
        /// </summary>
        public virtual IReadOnlyCollection<INavigationKeyDescriptor> NavigationKeyDescriptors { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is the agregate root on the navigation.
        /// </summary>
        /// <remarks>
        /// That the source entity or "from" entity is marked as an aggregate root does not means the entity
        /// is the aggregate root for a whole hierarchy of objects. This term is only describing the relationship
        /// between the "from" "to" entities.
        /// </remarks>
        /// <value>
        /// <c>true</c> if this instance is root agregate; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsAggregateRoot { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="fromDescriptor">From descriptor.</param>
        /// <param name="property">The property information.</param>
        private NavigationPropertyDescriptor(ITableTypeDescriptor fromDescriptor, PropertyDecoration property)
        {
            this.PropertyDecoration = property;
            this.FromDescriptor = fromDescriptor;
            this.PropertyInfo = property.PropertyInfo;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of <see cref="NavigationPropertyDescriptor"/> from a <see cref="PropertyDecoration"/> instance and a <see cref="TableTypeDescriptor"/>.
        /// </summary>
        /// <remarks>
        /// If the property is not decorated with the <see cref="NavigationAttribute"/> the method will return <c>null</c>.
        /// </remarks>
        /// <param name="fromDescriptor">The table type descriptor of the source property.</param>
        /// <param name="property">The property info to use.</param>
        /// <returns>A column property descriptor or null otherwise.</returns>
        internal static INavigationPropertyDescriptor Create(ITableTypeDescriptor fromDescriptor, PropertyDecoration property)
        {
            var descriptor = new NavigationPropertyDescriptor(fromDescriptor, property);
            return descriptor.Initialize() ? descriptor : null;
        }

        /// <summary>
        /// Creates a list of <see cref="NavigationPropertyDescriptor"/> from a collection of <see cref="PropertyDecoration"/> and a <see cref="TableTypeDescriptor"/>.
        /// </summary>
        /// <remarks>
        /// This method will trim all null appearances if any.
        /// <see cref="Create(ITableTypeDescriptor, Descriptors.PropertyDecoration)"/>
        /// </remarks>
        /// <param name="fromDescriptor">The table type descriptor of the source property.</param>
        /// <param name="properties">Collection of property info.</param>
        /// <returns>A list of <see cref="ColumnPropertyDescriptor"/></returns>
        internal static List<INavigationPropertyDescriptor> Create(ITableTypeDescriptor fromDescriptor, IEnumerable<PropertyDecoration> properties)
        {
            return properties.Select(x => Create(fromDescriptor, x)).Where(x => x != null).ToList();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Navigation Property Descriptor [{this.PropertyName}]";

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the navigation property descriptor.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the initialization was successfull; <c>false</c> otherwise.
        /// </returns>
        private bool Initialize()
        {
            this.NavigationAttributes = this.PropertyDecoration.GetAttributes<NavigationAttribute>().ToList();

            if (this.NavigationAttributes == null || !this.NavigationAttributes.Any())
                return false;

            this.PropertyName = this.PropertyInfo.Name;
            this.PropertyType = this.PropertyInfo.PropertyType;

            this.ToDescriptor = DescriptorCache.Instance.GetTableTypeDescriptor(this.NavigationAttributes.First().ReferencedType);

            this.NavigationKeyDescriptors = this.NavigationAttributes.Select(x => new NavigationKeyDescriptor(this.FromDescriptor, this.ToDescriptor, x.SourceProperty, x.ReferencedProperty)).ToList();
            this.IsAggregateRoot = this.PropertyType != typeof(string) && (this.PropertyType.IsArray || typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(this.PropertyType));
            return true;
        }

        #endregion
    }
}