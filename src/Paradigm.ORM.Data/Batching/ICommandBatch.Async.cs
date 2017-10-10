using System;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Batching
{
    internal partial interface ICommandBatch
    {
        /// <summary>
        /// Gets or sets a reference to an action that will be executed after the batch is executed.
        /// </summary>
        Func<Task> AfterExecuteAsync { get; set; }

        /// <summary>
        /// Executes the inner command.
        /// </summary>
        /// <remarks>
        /// If any of the command batches have a result callback, the
        /// inner command will be executed as DataReader operation and the
        /// callbacks will be secuentially executed.
        /// If no command step have a callback
        /// </remarks>
        Task ExecuteAsync();
    }
}