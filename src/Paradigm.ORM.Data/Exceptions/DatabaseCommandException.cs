using System;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when a command being executed fails.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class DatabaseCommandException : Exception
    {
        /// <summary>
        /// The default message.
        /// </summary>
        public const string DefaultMessage = "There was a problem executing the command.";

        /// <summary>
        /// Gets the database command.
        /// </summary>
        /// <value>
        /// The database command.
        /// </value>
        public IDatabaseCommand Command { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseCommandException"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="innerException">Inner exception.</param>
        public DatabaseCommandException(IDatabaseCommand command, Exception innerException): base(DefaultMessage, innerException)
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
        }
    }
}