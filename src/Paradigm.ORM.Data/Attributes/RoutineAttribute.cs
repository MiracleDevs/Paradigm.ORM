using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the class maps to a database routine.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class RoutineAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the catalog.
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or sets the name of the schema.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the name of the Routine.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineAttribute"/> class.
        /// </summary>
        public RoutineAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public RoutineAttribute(string name)
        {
            this.Name = name;

        }
    }
}