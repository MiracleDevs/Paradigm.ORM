using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.Batching
{
    /// <summary>
    /// Represent a single command batch, that manages several command steps.
    /// </summary>
    /// <remarks>
    /// The command batches allow to queue individual commands on a
    /// single steps, executing all the commands on a single roadtrip to the
    /// database.
    /// </remarks>
    /// <seealso cref="ICommandBatch" />
    /// <seealso cref="IBatchManager"/>
    /// <seealso cref="ICommandBatchStep"/>
    internal partial class CommandBatch : ICommandBatch
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current parameter count.
        /// </summary>
        private int ParameterCount { get; set; }

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        private string CommandText { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        private IDatabaseCommand Command { get; set; }

        /// <summary>
        /// Gets or sets the connector.
        /// </summary>
        private IDatabaseConnector Connector { get; set; }

        /// <summary>
        /// Gets or sets the format provider.
        /// </summary>
        private ICommandFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Gets the list of command steps.
        /// </summary>
        public List<ICommandBatchStep> Steps { get; }

        /// <summary>
        /// Gets or sets a reference to an action that will be executed after the batch is executed.
        /// </summary>
        public Action AfterExecute { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of commands per batch.
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// Gets or sets the current count.
        /// </summary>
        public int CurrentCount { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBatch"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public CommandBatch(IDatabaseConnector connector)
        {
            this.Connector = connector;
            this.FormatProvider = connector.GetCommandFormatProvider();
            this.Command = connector.CreateCommand();
            this.Steps = new List<ICommandBatchStep>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Command?.Dispose();

            this.Command = null;
            this.Connector = null;
            this.FormatProvider = null;
        }

        /// <summary>
        /// Adds a new command step to the step list.
        /// </summary>
        /// <param name="step">The step to add.</param>
        /// <returns>
        /// A reference to this batch instance.
        /// </returns>
        public ICommandBatch Add(ICommandBatchStep step)
        {
            if (this.MaxCount > 0 && this.CurrentCount >= this.MaxCount)
                return null;

            /* CURRENT TSQL LIMIT FOR SYNC QUERIES */
            if (this.ParameterCount + step.Command.Parameters.Count() >= 2100)
                return null;

            this.AddCommand(step.Command);

            if (step.BatchResultCallback != null || step.BatchResultCallbackAsync != null)
            {
                this.Steps.Add(step);
            }

            return this;
        }

        /// <summary>
        /// Gets the inner command.
        /// </summary>
        /// <returns>
        /// The inner command.
        /// </returns>
        /// <remarks>
        /// The command batch works merging multiple individual commands into one big command.
        /// For each command added, the command text will be concatenated to the inner command,
        /// and the parameters will be queued inside the inner parameter, changing their names
        /// to prevent merging conflicts.
        /// </remarks>
        public IDatabaseCommand GetCommand()
        {
            this.Command.CommandText = this.CommandText;
            return this.Command;
        }

        /// <summary>
        /// Executes the inner command.
        /// </summary>
        /// <remarks>
        /// If any of the command batches have a result callback, the
        /// inner command will be executed as DataReader operation and the
        /// callbacks will be secuentially executed.
        /// If no command step have a callback
        /// </remarks>
        public void Execute()
        {
            var command = this.GetCommand();

            if (this.Steps.Any())
                this.Connector.ExecuteReader(command, this.ProcessDataReader);
            else
                this.Connector.ExecuteNonQuery(command);

            this.AfterExecute?.Invoke();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Processes the data reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        private void ProcessDataReader(IDatabaseReader reader)
        {
            // iterate over all the batch steps.
            foreach (var result in this.Steps)
            {
                // call the callback and consume the resultset.
                result.BatchResultCallback.Invoke(reader);

                if (!reader.NextResult())
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <remarks>
        /// 1. Concatenate the previous commands to the new one, and add a query separator.
        //  2. Then iterate over the parameters in the new command, and replace the name for
        //     for an incremental name, to be able to support many similar queries (Insert, Update, etc).
        //  3. Refresh the CommandText after generating and updating the batch command.
        /// </remarks>
        private void AddCommand(IDatabaseCommand command)
        {
            var builder = new StringBuilder();

            builder.Append(command.CommandText);
            builder.Append(this.FormatProvider.GetQuerySeparator());

            foreach (var parameter in command.Parameters)
            {
                var oldName = parameter.ParameterName;
                var newName = $"@p{this.ParameterCount++}";

                builder.Replace(oldName, newName);

                this.Command.AddParameter(newName, parameter.DbType, parameter.Size, parameter.Precision, parameter.Scale, parameter.IsNullable).Value = parameter.Value;
            }

            this.CommandText = $"{this.CommandText}{builder}";
        }

        #endregion
    }
}