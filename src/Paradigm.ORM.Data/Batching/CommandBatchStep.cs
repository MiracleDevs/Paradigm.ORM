using System;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Batching
{
    /// <summary>
    /// Represents a single steps inside a command batch.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Batching.ICommandBatchStep" />
    public class CommandBatchStep: ICommandBatchStep
    {
        #region Properties

        /// <summary>
        /// Gets the command.
        /// </summary>
        public IDatabaseCommand Command { get; private set; }

        /// <summary>
        /// Gets the batch result callback.
        /// </summary>
        public Action<IDatabaseReader> BatchResultCallback { get; }

        /// <summary>
        /// Gets the asynchronous batch result callback.
        /// </summary>
        public Func<IDatabaseReader, Task> BatchResultCallbackAsync { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBatchStep"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        public CommandBatchStep(IDatabaseCommand command)
        {
            this.Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBatchStep"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="batchResultCallback">The batch result callback.</param>
        public CommandBatchStep(IDatabaseCommand command, Action<IDatabaseReader> batchResultCallback)
        {
            this.Command = command;
            this.BatchResultCallback = batchResultCallback;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBatchStep"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="batchResultCallbackAsync">The batch result callback asynchronous.</param>
        public CommandBatchStep(IDatabaseCommand command, Func<IDatabaseReader, Task> batchResultCallbackAsync)
        {
            this.Command = command;
            this.BatchResultCallbackAsync = batchResultCallbackAsync;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Command?.Dispose();
            this.Command = null;
        }

        #endregion
    }
}