using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    public partial interface INavigationDatabaseAccess
    {
        /// <summary>
        /// Selects the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        Task SelectAsync(IEnumerable<object> entities);

        /// <summary>
        /// Saves one to one related entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        Task SaveBeforeAsync(IEnumerable<object> entities);

        /// <summary>
        /// Saves the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        Task SaveAfterAsync(IEnumerable<object> entities);

        /// <summary>
        /// Deletes one to one relation entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        Task DeleteBeforeAsync(IEnumerable<object> entities);

        /// <summary>
        /// Deletes the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        Task DeleteAfterAsync(IEnumerable<object> entities);
    }
}