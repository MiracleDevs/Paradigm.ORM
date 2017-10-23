namespace Paradigm.ORM.Data.Database
{
    /// <summary>
    /// Provides agnostic database configuration for the shared data library.
    /// </summary>
    public interface IDatabaseConfiguration
    {
        /// <summary>
        /// Gets the maximum number of commands per batch that the database supports.
        /// </summary>
        /// <value>
        /// The commands per batch.
        /// </value>
        int MaxCommandsPerBatch { get; }

        /// <summary>
        /// Gets the maximum number of parameters per command that the database supports.
        /// </summary>
        /// <value>
        /// The parameters per command.
        /// </value>
        int MaxParametersPerCommand { get; }

        /// <summary>
        /// Gets the maximum size of the command.
        /// </summary>
        /// <value>
        /// The maximum size of the command.
        /// </value>
        int MaxCommandLength { get; }
    }
}