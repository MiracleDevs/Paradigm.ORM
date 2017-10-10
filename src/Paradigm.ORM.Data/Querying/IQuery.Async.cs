using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Querying
{
    public partial interface IQuery<TResultType>
    {
        /// <summary>
        /// Executes the specified query and returns a list of <see cref="TResultType"/>.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A list of <see cref="TResultType"/>.</returns>
        Task<List<TResultType>> ExecuteAsync(string whereClause = null, params object[] parameters);
    }
}