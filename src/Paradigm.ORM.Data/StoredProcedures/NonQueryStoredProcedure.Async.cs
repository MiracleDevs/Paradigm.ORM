using System;
using Paradigm.ORM.Data.Extensions;
using System.Data;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial class NonQueryStoredProcedure<TParameters>
    {
        #region Public Methods

        /// <summary>
        /// Executes the stored procedure as a non query.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Number of affected rows.
        /// </returns>
        public async Task<int> ExecuteNonQueryAsync(TParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("Must give parameters to execute the stored procedure.");

            using (var command = this.Connector.CreateCommand(this.GetRoutineName(), CommandType.StoredProcedure))
            {
                this.PopulateParameters(command, parameters);
                return await command.ExecuteNonQueryAsync();
            }
        }

        #endregion
    }
}