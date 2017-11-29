using System;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when a .net type in a class is not supported by the orm.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmTypeNotSupportedException : Exception
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmTypeNotSupportedException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public OrmTypeNotSupportedException(Type type, string message) : base(message)
        {
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmTypeNotSupportedException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public OrmTypeNotSupportedException(Type type, string message, Exception innerException) : base(message, innerException)
        {
            this.Type = type;
        }
    }
}