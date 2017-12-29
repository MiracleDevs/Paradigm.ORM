using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Batching
{
    /// <summary>
    /// Provides an interface for command batches.
    /// </summary>
    /// <remarks>
    /// The command batches allow to queue individual commands on a
    /// single steps, executing all the commands on a single roadtrip to the
    /// database.
    /// </remarks>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="IBatchManager"/>
    /// <seealso cref="ICommandBatchStep"/>
    public partial interface ICommandBatch : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the maximum count of commands per batch.
        /// </summary>
        int MaxCount { get; set; }

        /// <summary>
        /// Gets or sets the current count.
        /// </summary>
        int CurrentCount { get; }

        /// <summary>
        /// Gets the list of command steps.
        /// </summary>
        List<ICommandBatchStep> Steps { get; }

        /// <summary>
        /// Gets or sets a reference to an action that will be executed after the batch is executed.
        /// </summary>
        Action AfterExecute { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new command step to the step list.
        /// </summary>
        /// <param name="step">The step to add.</param>
        /// <returns>A reference to this batch instance.</returns>
        ICommandBatch Add(ICommandBatchStep step);

        /// <summary>
        /// Gets the inner command.
        /// </summary>
        /// <remarks>
        /// The command batch works merging multiple individual commands into one big command.
        /// For each command added, the command text will be concatenated to the inner command,
        /// and the parameters will be queued inside the inner parameter, changing their names
        /// to prevent merging conflicts.
        /// </remarks>
        /// <returns>The inner command.</returns>
        IDatabaseCommand GetCommand();

        /// <summary>
        /// Executes the inner command.
        /// </summary>
        /// <remarks>
        /// If any of the command batches have a result callback, the
        /// inner command will be executed as DataReader operation and the
        /// callbacks will be secuentially executed.
        /// If no command step have a callback
        /// </remarks>
        void Execute();

        #endregion
    }
}