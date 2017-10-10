using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.StoredProcedures
{
    public partial interface IReaderStoredProcedure<in TParameters, TResult1, TResult2, TResult3, TResult4, TResult5>
    {
        /// <summary>
        /// Executes the stored procedure and return a list of tuples.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>List of tuples.</returns>
        Task<Tuple<List<TResult1>, List<TResult2>, List<TResult3>, List<TResult4>, List<TResult5>>> ExecuteAsync(TParameters parameters);
    }
}