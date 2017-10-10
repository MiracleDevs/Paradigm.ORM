using System.Threading.Tasks;

namespace Paradigm.ORM.Data.ValueProviders
{
    public partial class ClassValueProvider
    {
        #region Public Methods

        /// <summary>
        /// Moves the reading cursor to the next entity or row.
        /// </summary>
        /// <returns>True if there are more entities to read, false otherwise.</returns>
        public Task<bool> MoveNextAsync()
        {
            return Task.FromResult(this.MoveNext());
        }

        #endregion
    }
}