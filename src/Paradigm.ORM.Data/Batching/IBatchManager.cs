using System;

namespace Paradigm.ORM.Data.Batching
{
    /// <summary>
    /// Provides an interface for batch managers.
    /// </summary>
    /// <remarks>
    /// The function of a batch manager is to manage secuential batches
    /// that may require actions inbetween.
    /// For example when saving a list of entities, if the entities have
    /// children, we need to execute the first batch of parents, then extract
    /// the ids, set the ids on children, and then execute the second batch for
    /// the children.
    /// </remarks>
    /// <remarks>
    /// Some databases pose limits to the maximum characters of a single command batch,
    /// so the system will create new batch each time the current steps has reached
    /// the maximum character size.
    /// If the user manually sets the <see cref="MaxCount"/> value, the system will also use
    /// this value to create new batches when it reaches the max count.
    /// </remarks>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="ICommandBatch"/>
    /// <seealso cref="ICommandBatchStep"/>
    public partial interface IBatchManager : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the maximum count of command steps per batch.
        /// </summary>
        int MaxCount { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified command to the current batch.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Returns the batch manager instance.</returns>
        IBatchManager Add(ICommandBatchStep command);

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
        IBatchManager AddNewBatch(Action action);

        /// <summary>
        /// Adds a new batch to the manager.
        /// </summary>
        /// <returns>Returns the batch manager instance.</returns>
        /// <remarks>
        /// The batch manager can handle several batches.
        /// When adding commands to manager, the manager will add the command
        /// to the current active batch manager as new batch step.
        /// </remarks>
        IBatchManager AddNewBatch();

        /// <summary>
        /// Executes all the batch steps.
        /// </summary>
        void Execute();

        /// <summary>
        /// Resets the batch manager.
        /// All the steps and command will be cleared.
        /// </summary>
        void Reset();

        #endregion
    }
}