using System;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Generic exception thrown from within the orm connector class.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmConnectorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrmConnectorException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OrmConnectorException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmConnectorException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public OrmConnectorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}