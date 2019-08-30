using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Database
{
    public partial interface IDatabaseConnector
    {
        /// <summary>
        /// Opens the connection to a database.
        /// </summary>
        Task OpenAsync();

        /// <summary>
        /// Closes a previously opened connection to a database.
        /// </summary>
        Task CloseAsync();
    }
}