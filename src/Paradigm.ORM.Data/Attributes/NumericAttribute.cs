using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the property maps to a numeric database column.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NumericAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the numeric scale.
        /// </summary>
        public byte Scale { get; set; }

        /// <summary>
        /// Gets or sets the numeric precision.
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericAttribute"/> class.
        /// </summary>
        public NumericAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericAttribute"/> class.
        /// </summary>
        /// <param name="precision">The numeric precision.</param>
        /// <param name="scale">The numeric scale.</param>
        public NumericAttribute(byte precision, byte scale)
        {
            this.Scale = scale;
            this.Precision = precision;
        }
    }
}