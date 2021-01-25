using Paradigm.ORM.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial class ReaderStoredProcedure<TParameters, TResult1, TResult2>
    {
        #region Public Methods

        /// <summary>
        /// Executes the stored procedure and return a list of tuples.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// List of tuples.
        /// </returns>
        public async Task<Tuple<List<TResult1>, List<TResult2>>> ExecuteAsync(TParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters), "Must give parameters to execute the stored procedure.");

            using var command = this.Connector.CreateCommand(this.GetRoutineName(), CommandType.StoredProcedure);
            this.PopulateParameters(command, parameters);

            using var reader = await command.ExecuteReaderAsync();
            var result1 = await this.Mapper1.MapAsync(reader);
            await reader.NextResultAsync();
            var result2 = await this.Mapper2.MapAsync(reader);

            return new Tuple<List<TResult1>, List<TResult2>>(result1, result2);
        }

        #endregion
    }
}