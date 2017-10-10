using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial interface IReaderStoredProcedure<in TParameters, TResult1>
    {
        /// <summary>
        /// Executes the stored procedure and return a list of <see cref="TResult1"/>.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>List of <see cref="TResult1"/></returns>
        Task<List<TResult1>> ExecuteAsync(TParameters parameters);
    }
}