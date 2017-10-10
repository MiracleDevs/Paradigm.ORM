using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the property maps to a column with a maximum size.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SizeAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the maximum size.
        /// </summary>
        /// <value>
        /// The maximum size.
        /// </value>
        public long MaxSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeAttribute"/> class.
        /// </summary>
        public SizeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeAttribute"/> class.
        /// </summary>
        /// <param name="maxSize">The maximum size.</param>
        public SizeAttribute(long maxSize)
        {
            this.MaxSize = maxSize;
        }
    }
}