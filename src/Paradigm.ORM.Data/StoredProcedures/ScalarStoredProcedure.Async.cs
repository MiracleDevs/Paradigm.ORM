using Paradigm.ORM.Data.Extensions;
using System;
using System.Data;
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

            using (var command = this.Connector.CreateCommand(this.GetRoutineName(), CommandType.StoredProcedure))
            {
                this.PopulateParameters(command, parameters);
                return (TResult)await command.ExecuteScalarAsync();
            }
        }

        #endregion
    }
}