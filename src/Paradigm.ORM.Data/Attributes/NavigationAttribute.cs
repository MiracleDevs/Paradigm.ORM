using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the property is "navigates" to another type.
    /// </summary>
    /// <remarks>
    /// This is common on Parent / child relationships.
    /// </remarks>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class NavigationAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the referenced type.
        /// </summary>
        /// <value>
        /// To type.
        /// </value>
        public Type ReferencedType { get; set; }

        /// <summary>
        /// Gets or sets the name of the source property.
        /// </summary>
        /// <value>
        /// From property.
        /// </value>
        public string SourceProperty { get; set; }

        /// <summary>
        /// Gets or sets the name of the referenced property.
        /// </summary>
        /// <value>
        /// To property.
        /// </value>
        public string ReferencedProperty { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationAttribute"/> class.
        /// </summary>
        public NavigationAttribute()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationAttribute"/> class.
        /// </summary>
        /// <param name="referencedType">The name of the referenced type.</param>
        /// <param name="sourceProperty">The name of the source property.</param>
        /// <param name="referencedProperty">The name of the referenced property.</param>
        public NavigationAttribute(Type referencedType, string sourceProperty, string referencedProperty)
        {
            this.ReferencedType = referencedType;
            this.SourceProperty = sourceProperty;
            this.ReferencedProperty = referencedProperty;
        }
    }
}