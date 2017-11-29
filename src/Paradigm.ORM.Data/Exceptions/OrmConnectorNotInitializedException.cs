using System;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when the database connector wants to be used but the connection was not initialized.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmConnectorNotInitializedException : Exception
    {
        /// <summary>
        /// The default message.
        /// </summary>
        public const string DefaultMessage = "The connector has not been initialized.";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmConnectorNotInitializedException"/> class.
        /// </summary>
        public OrmConnectorNotInitializedException() : this(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmConnectorNotInitializedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OrmConnectorNotInitializedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmConnectorNotInitializedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public OrmConnectorNotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}