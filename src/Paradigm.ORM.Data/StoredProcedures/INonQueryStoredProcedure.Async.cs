using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial interface INonQueryStoredProcedure<in TParameters>
    {
        /// <summary>
        /// Executes the stored procedure as a non query.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Number of affected rows.</returns>
        Task<int> ExecuteNonQueryAsync(TParameters parameters);
    }
}