using System.Collections.Generic;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Mappers
{
    public partial interface IDatabaseReaderMapper
    {
        /// <summary>
        /// Gets a list of objects already mapped from a known table, to a known object type.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A list of mapped objects.</returns>
        Task<List<object>> MapAsync(IDatabaseReader reader);
    }
}