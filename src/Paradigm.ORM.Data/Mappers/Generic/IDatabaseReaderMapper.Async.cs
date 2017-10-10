using System.Collections.Generic;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Mappers.Generic
{
    public partial interface IDatabaseReaderMapper<TEntity> 
    {
        /// <summary>
        /// Gets a list of <see cref="TEntity"/> from a <see cref="IDatabaseReader"/>.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A list of <see cref="TEntity"/>.</returns>
        new Task<List<TEntity>> MapAsync(IDatabaseReader reader);
    }
}