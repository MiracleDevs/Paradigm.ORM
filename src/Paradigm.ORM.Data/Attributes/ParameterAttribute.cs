using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the property maps to a stored procedure parameter.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the data type of the parameter.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an input or an output.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this parameter is an input parameter; otherwise, <c>false</c>.
        /// </value>
        public bool IsInput { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterAttribute"/> class.
        /// </summary>
        public ParameterAttribute()
        {
            this.IsInput = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public ParameterAttribute(string name)
        {
            this.Name = name;
            this.IsInput = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="isInput"><c>true</c> if the parameter is an input parameter.</param>
        public ParameterAttribute(string name, string type, bool isInput)
        {
            this.Name = name;
            this.Type = type;
            this.IsInput = isInput;
        }
    }
}