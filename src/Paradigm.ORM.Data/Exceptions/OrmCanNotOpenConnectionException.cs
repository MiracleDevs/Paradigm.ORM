using System;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when the connection can not be opened.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmCanNotOpenConnectionException : Exception
    {
        /// <summary>
        /// The default message.
        /// </summary>
        public const string DefaultMessage = "The connection to the database can not be opened.";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmCanNotOpenConnectionException"/> class.
        /// </summary>
        public OrmCanNotOpenConnectionException(): this(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmCanNotOpenConnectionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OrmCanNotOpenConnectionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmCanNotOpenConnectionException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public OrmCanNotOpenConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}