using System;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial class ScalarStoredProcedure<TParameters, TResult>
    {
        #region Public Methods

        /// <summary>
        /// Executes the stored procedure and returns a scalar value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The scalar value.
        /// </returns>
        public async Task<TResult> ExecuteScalarAsync(TParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("Must give parameters to execute the stored procedure.");

            this.SetParametersValue(parameters);
            return (TResult) await this.Command.ExecuteScalarAsync();
        }

        #endregion
    }
}