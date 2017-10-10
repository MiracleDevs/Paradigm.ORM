using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the class maps to a database Routine, but the mapping information is contained on a different .NET type.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="RoutineAttribute"/>
    public class RoutineTypeAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the type mapping.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineTypeAttribute"/> class.
        /// </summary>
        public RoutineTypeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineTypeAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RoutineTypeAttribute(Type type)
        {
            this.Type = type;
        }
    }
}