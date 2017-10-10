using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the class maps to a database table or view, but the mapping information is contained on a different .NET type.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="TableAttribute"/>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableTypeAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the type mapping.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableTypeAttribute"/> class.
        /// </summary>
        public TableTypeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableTypeAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public TableTypeAttribute(Type type)
        {
            this.Type = type;
        }
    }
}