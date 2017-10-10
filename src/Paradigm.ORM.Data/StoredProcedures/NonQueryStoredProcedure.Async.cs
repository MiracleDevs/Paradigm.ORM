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
            this.SetParametersValue(parameters);
            return await this.Command.ExecuteNonQueryAsync();
        }

        #endregion
    }
}