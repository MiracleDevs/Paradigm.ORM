using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Batching
{
    /// <summary>
    /// Represent a batch manager.
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
    /// <seealso cref="IBatchManager" />
    /// <seealso cref="ICommandBatch"/>
    /// <seealso cref="ICommandBatchStep"/>
    public partial class BatchManager : IBatchManager
    {
        #region Properties

        /// <summary>
        /// Gets the command batches.
        /// </summary>
        private List<ICommandBatch> CommandBatches { get; }

        /// <summary>
        /// Gets or sets the current batch.
        /// </summary>
        private ICommandBatch CurrentBatch { get; set; }

        /// <summary>
        /// Gets or sets the connector.
        /// </summary>
        private IDatabaseConnector Connector { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of command steps per batch.
        /// </summary>
        public int MaxCount { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchManager"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public BatchManager(IDatabaseConnector connector)
        {
            this.Connector = connector;
            this.CommandBatches = new List<ICommandBatch>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Reset();
            this.CurrentBatch = null;
            this.Connector = null;
        }

        /// <summary>
        /// Adds the specified command to the current batch.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        /// Returns the batch manager instance.
        /// </returns>
        /// <exception cref="System.Exception">The command can not be added to the batch, probably it has too many parameters.</exception>
        public IBatchManager Add(ICommandBatchStep command)
        {
            var batch = this.GetCurrentBatch().Add(command);

            if (batch == null)
            {
                this.CurrentBatch = null;
                batch = this.GetCurrentBatch().Add(command);

                if (batch == null)
                {
                    throw new Exception("The command can not be added to the batch, probably it has too many parameters.");
                }
            }

            return this;
        }

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
        public IBatchManager AddNewBatch(Action action)
        {
            this.GetCurrentBatch().AfterExecute = action;
            this.CurrentBatch = null;
            return this;
        }

        /// <summary>
        /// Adds a new batch to the manager.
        /// </summary>
        /// <returns>
        /// Returns the batch manager instance.
        /// </returns>
        /// <remarks>
        /// The batch manager can handle several batches.
        /// When adding commands to manager, the manager will add the command
        /// to the current active batch manager as new batch step.
        /// </remarks>
        public IBatchManager AddNewBatch()
        {
            this.CurrentBatch = null;
            return this;
        }

        /// <summary>
        /// Executes all the batch steps.
        /// </summary>
        public void Execute()
        {
            // we need to use a for instead of a foreach, because
            // the collection is mutable. Batches will insert other
            // batches if required.
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < this.CommandBatches.Count; i++)
            {
                this.CommandBatches[i].Execute();
            }

            this.Reset();
        }

        /// <summary>
        /// Resets the batch manager.
        /// All the steps and command will be cleared.
        /// </summary>
        public void Reset()
        {
            this.CommandBatches.ForEach(x => x?.Dispose());
            this.CommandBatches.Clear();
            this.CurrentBatch = null;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Gets the current batch.
        /// </summary>
        /// <remarks>
        /// If the current batch is null, will create, add and return a new batch.
        /// </remarks>
        /// <returns>Current batch instance</returns>
        private ICommandBatch GetCurrentBatch()
        {
            if (this.CurrentBatch != null)
                return this.CurrentBatch;

            this.CurrentBatch = new CommandBatch(this.Connector) { MaxCount = this.MaxCount };
            this.CommandBatches.Add(this.CurrentBatch);

            return this.CurrentBatch;
        }

        #endregion
    }
}