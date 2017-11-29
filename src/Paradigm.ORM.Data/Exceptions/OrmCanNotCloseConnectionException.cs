using System;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when the connection can not be closed.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmCanNotCloseConnectionException : Exception
    {
        /// <summary>
        /// The default message.
        /// </summary>
        public const string DefaultMessage = "The connection to the database can not be closed.";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmCanNotCloseConnectionException"/> class.
        /// </summary>
        public OrmCanNotCloseConnectionException(): this(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmCanNotCloseConnectionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OrmCanNotCloseConnectionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmCanNotCloseConnectionException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public OrmCanNotCloseConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}