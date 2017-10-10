using System;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Batching
{
    /// <summary>
    /// Provides an interface for command batch steps.
    /// </summary>
    /// <remarks>
    /// A command batch step represents a single command inside
    /// the the batch, and a result callback that will be called
    /// after the whole batchs is executed.
    /// </remarks>
    public interface ICommandBatchStep: IDisposable
    {
        /// <summary>
        /// Gets the command.
        /// </summary>
        IDatabaseCommand Command { get; }

        /// <summary>
        /// Gets the batch result callback.
        /// </summary>
        Action<IDatabaseReader> BatchResultCallback { get; }

        /// <summary>
        /// Gets the asynchronous batch result callback.
        /// </summary>
        Func<IDatabaseReader, Task> BatchResultCallbackAsync { get; }
    }
}