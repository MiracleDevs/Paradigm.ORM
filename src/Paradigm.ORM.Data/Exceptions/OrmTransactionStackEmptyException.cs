using System;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when the connector is asked to pop the transaction stack but its empty.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmTransactionStackEmptyException : Exception
    {
        /// <summary>
        /// The default message.
        /// </summary>
        public const string DefaultMessage = "The transaction stack is empty.";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmTransactionStackEmptyException"/> class.
        /// </summary>
        public OrmTransactionStackEmptyException() : this(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmTransactionStackEmptyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OrmTransactionStackEmptyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmTransactionStackEmptyException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public OrmTransactionStackEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}