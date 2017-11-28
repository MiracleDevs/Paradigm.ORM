using Paradigm.ORM.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial class ReaderStoredProcedure<TParameters, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>
    {
        #region Public Methods

        /// <summary>
        /// Executes the stored procedure and return a list of tuples.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// List of tuples.
        /// </returns>
        public async Task<Tuple<List<TResult1>, List<TResult2>, List<TResult3>, List<TResult4>, List<TResult5>, List<TResult6>>> ExecuteAsync(TParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("Must give parameters to execute the stored procedure.");

            using (var command = this.Connector.CreateCommand(this.GetRoutineName(), CommandType.StoredProcedure))
            {
                this.PopulateParameters(command, parameters);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var result1 = await this.Mapper1.MapAsync(reader);
                    await reader.NextResultAsync();
                    var result2 = await this.Mapper2.MapAsync(reader);
                    await reader.NextResultAsync();
                    var result3 = await this.Mapper3.MapAsync(reader);
                    await reader.NextResultAsync();
                    var result4 = await this.Mapper4.MapAsync(reader);
                    await reader.NextResultAsync();
                    var result5 = await this.Mapper5.MapAsync(reader);
                    await reader.NextResultAsync();
                    var result6 = await this.Mapper6.MapAsync(reader);

                    return new Tuple<List<TResult1>, List<TResult2>, List<TResult3>, List<TResult4>, List<TResult5>, List<TResult6>>(result1, result2, result3, result4, result5, result6);
                }
            }
        }

        #endregion
    }
}