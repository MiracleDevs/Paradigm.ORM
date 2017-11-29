using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Internal class used to store information about properties and their decorator attributes.
    /// </summary>
    internal class PropertyDecoration
    {
        #region Properties

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <value>
        /// The property information.
        /// </value>
        internal PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        internal List<Attribute> Attributes { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDecoration"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="attributes">The attributes.</param>
        /// <exception cref="ArgumentNullException">
        /// propertyInfo - propertyInfo
        /// or
        /// attributes - attributes
        /// </exception>
        public PropertyDecoration(PropertyInfo propertyInfo, List<Attribute> attributes)
        {
            this.PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo), $"The {nameof(propertyInfo)} can not be null.");
            this.Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes), $"The {nameof(attributes)} can not be null.");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns></returns>
        public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
        {
            return this.Attributes.FirstOrDefault(x => x is TAttribute) as TAttribute;
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns></returns>
        public IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute
        {
            return this.Attributes.Where(x => x is TAttribute).Cast<TAttribute>();
        }

        #endregion
    }
}