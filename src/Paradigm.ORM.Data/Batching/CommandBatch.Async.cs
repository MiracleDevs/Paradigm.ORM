using System;
using System.Linq;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.Batching
{
    public partial class CommandBatch
    {
        #region Properties

        /// <summary>
        /// Gets or sets a reference to an action that will be executed after the batch is executed.
        /// </summary>
        public Func<Task> AfterExecuteAsync { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the inner command.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// If any of the command batches have a result callback, the
        /// inner command will be executed as DataReader operation and the
        /// callbacks will be secuentially executed.
        /// If no command step have a callback
        /// </remarks>
        public async Task ExecuteAsync()
        {
            var command = this.GetCommand();

            if (this.Steps.Any())
                await this.Connector.ExecuteReaderAsync(command, async x => await this.ProcessDataReaderAsync(x));
            else
                await this.Connector.ExecuteNonQueryAsync(command);

            if (this.AfterExecuteAsync != null)
            {
                await this.AfterExecuteAsync.Invoke();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Processes the data reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        private async Task ProcessDataReaderAsync(IDatabaseReader reader)
        {
            // iterate over all the batch steps.
            foreach (var result in this.Steps)
            {
                // call the callback and consume the resultset.
                await result.BatchResultCallbackAsync.Invoke(reader);

                if (!await reader.NextResultAsync())
                {
                    break;
                }
            }
        }

        #endregion
    }
}