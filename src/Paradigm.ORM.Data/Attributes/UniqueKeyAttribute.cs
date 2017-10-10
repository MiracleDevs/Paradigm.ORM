using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the property is part of a database unique key.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class UniqueKeyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the unique key.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueKeyAttribute"/> class.
        /// </summary>
        public UniqueKeyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueKeyAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the unique key.</param>
        /// <param name="column">The name of the column.</param>
        public UniqueKeyAttribute(string name, string column)
        {
            this.Name = name;
            this.Column = column;
        }
    }
}