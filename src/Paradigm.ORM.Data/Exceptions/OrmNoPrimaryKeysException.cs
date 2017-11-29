using System;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when the table does not have any primary information.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OrmNoPrimaryKeysException : Exception
    {
        /// <summary>
        /// The default message.
        /// </summary>
        public const string DefaultMessage = "The table '{0}' does not have primary keys defined.";

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public string Table { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmNoPrimaryKeysException"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public OrmNoPrimaryKeysException(string table): this(table, string.Format(DefaultMessage, table))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmNoPrimaryKeysException"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="message">The message.</param>
        public OrmNoPrimaryKeysException(string table, string message) : base(message)
        {
            this.Table = table;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmNoPrimaryKeysException"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public OrmNoPrimaryKeysException(string table, string message, Exception innerException) : base(message, innerException)
        {
            this.Table = table;
        }
    }
}