using System;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when a table does not have mapping exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmMissingTableMappingException : Exception
    {
        /// <summary>
        /// The default message.
        /// </summary>
        public const string DefaultMessage = "The class or interface does not have table mapping information.";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmMissingTableMappingException"/> class.
        /// </summary>
        public OrmMissingTableMappingException() : this(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmMissingTableMappingException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OrmMissingTableMappingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmMissingTableMappingException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public OrmMissingTableMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}