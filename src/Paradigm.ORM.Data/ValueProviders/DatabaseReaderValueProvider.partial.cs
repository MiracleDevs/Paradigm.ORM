using System.Threading.Tasks;

namespace Paradigm.ORM.Data.ValueProviders
{
    public partial class DatabaseReaderValueProvider
    {
        /// <summary>
        /// Moves the reading cursor to the next entity or row.
        /// </summary>
        /// <returns>True if can move to another row, false otherwise.</returns>
        public async Task<bool> MoveNextAsync()
        {
            return await this.Reader.ReadAsync();
        }
    }
}