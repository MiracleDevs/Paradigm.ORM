using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial interface IScalarStoredProcedure<in TParameters, TResult>
    {
        /// <summary>
        /// Executes the stored procedure and returns a scalar value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The scalar value.</returns>
        Task<TResult> ExecuteScalarAsync(TParameters parameters);
    }
}