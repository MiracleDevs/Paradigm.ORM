using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Mappers.Generic
{
    public partial class DatabaseReaderMapper<TEntity>
    {
        #region Public Methods

        /// <summary>
        /// Gets a list of <see cref="TEntity"/> mapped from a database reader.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A list of <see cref="TEntity"/>.</returns>
        public new async Task<List<TEntity>> MapAsync(IDatabaseReader reader)
        {
            return (await base.MapAsync(reader)).Cast<TEntity>().ToList();
        }

        #endregion
    }
}