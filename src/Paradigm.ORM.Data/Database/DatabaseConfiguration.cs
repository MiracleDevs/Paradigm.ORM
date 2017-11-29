namespace Paradigm.ORM.Data.Database
{
    /// <summary>
    /// Provides agnostic database configuration for the shared data library.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.IDatabaseConfiguration" />
    public class DatabaseConfiguration: IDatabaseConfiguration
    {
        /// <summary>
        /// Gets the maximum number of commands per batch that the database supports.
        /// </summary>
        /// <value>
        /// The commands per batch.
        /// </value>
        public int MaxCommandsPerBatch { get; }

        /// <summary>
        /// Gets the maximum number of parameters per command that the database supports.
        /// </summary>
        /// <value>
        /// The parameters per command.
        /// </value>
        public int MaxParametersPerCommand { get; }

        /// <summary>
        /// Gets the maximum size of the command.
        /// </summary>
        /// <value>
        /// The maximum size of the command.
        /// </value>
        public int MaxCommandLength { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConfiguration"/> class.
        /// </summary>
        /// <param name="maxCommandsPerBatch">The maximum commands per batch.</param>
        /// <param name="maxParametersPerCommand">The maximum parameters per command.</param>
        /// <param name="maxCommandSize">Maximum size of the command.</param>
        public DatabaseConfiguration(int maxCommandsPerBatch, int maxParametersPerCommand, int maxCommandSize)
        {
            this.MaxCommandsPerBatch = maxCommandsPerBatch;
            this.MaxParametersPerCommand = maxParametersPerCommand;
            this.MaxCommandLength = maxCommandSize;
        }
    }
}