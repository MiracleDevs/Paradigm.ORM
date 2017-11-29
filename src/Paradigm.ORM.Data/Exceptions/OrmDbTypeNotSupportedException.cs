using System;
using System.Data;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when a database type in a table or routine is not supported by the orm.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmDbTypeNotSupportedException : Exception
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public DbType Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmDbTypeNotSupportedException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public OrmDbTypeNotSupportedException(DbType type, string message) : base(message)
        {
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmDbTypeNotSupportedException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public OrmDbTypeNotSupportedException(DbType type, string message, Exception innerException) : base(message, innerException)
        {
            this.Type = type;
        }
    }
}