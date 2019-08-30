namespace Paradigm.ORM.Data.Logging
{
    /// <summary>
    /// Provides an interface for a log provider.
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warning(string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);
    }
}