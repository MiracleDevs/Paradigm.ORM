using System;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Batching
{
    public partial class BatchManager
    {
        #region Public Methods

        /// <summary>
        /// Adds a new batch to the manager.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        /// Returns the batch manager instance.
        /// </returns>
        /// <remarks>
        /// The batch manager can handle several sequential batches.
        /// When adding commands to manager, the manager will add the command
        /// to the current active batch as a new batch step.
        /// </remarks>
        public IBatchManager AddNewBatch(Func<Task> action)
        {
            this.GetCurrentBatch().AfterExecuteAsync = action;
            this.CurrentBatch = null;
            return this;
        }

        /// <summary>
        /// Executes all the batch steps.
        /// </summary>
        public async Task ExecuteAsync()
        {
            // we need to use a for instead of a foreach, because
            // the collection is mutable. Batches will insert other
            // batches if required.
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < this.CommandBatches.Count; i++)
            {
                await this.CommandBatches[i].ExecuteAsync();
            }

            this.Reset();
        }

        #endregion
    }
}