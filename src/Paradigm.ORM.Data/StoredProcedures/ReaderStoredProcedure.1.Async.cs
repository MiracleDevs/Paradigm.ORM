using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial class ReaderStoredProcedure<TParameters, TResult>
    {
        #region Public Methods

        /// <summary>
        /// Executes the stored procedure and return a list of <see cref="TResult" />.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// List of <see cref="TResult" />
        /// </returns>
        public async Task<List<TResult>> ExecuteAsync(TParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("Must give parameters to execute the stored procedure.");

            this.SetParametersValue(parameters);

            using (var reader = await this.Command.ExecuteReaderAsync())
            {
                return await this.Mapper.MapAsync(reader);
            }
        }

        #endregion
    }
}