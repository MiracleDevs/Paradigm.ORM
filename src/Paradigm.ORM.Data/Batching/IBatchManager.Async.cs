using System;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Batching
{
    public partial interface IBatchManager
    {
        /// <summary>
        /// Adds a new batch to the manager.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Returns the batch manager instance.</returns>
        /// <remarks>
        /// The batch manager can handle several sequential batches.
        /// When adding commands to manager, the manager will add the command
        /// to the current active batch as a new batch step.
        /// </remarks>
        IBatchManager AddNewBatch(Func<Task> action);

        /// <summary>
        /// Executes all the batch steps.
        /// </summary>
        Task ExecuteAsync();
    }
}