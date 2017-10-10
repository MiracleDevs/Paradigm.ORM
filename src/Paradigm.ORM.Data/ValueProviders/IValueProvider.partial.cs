using System.Threading.Tasks;

namespace Paradigm.ORM.Data.ValueProviders
{
    public partial interface IValueProvider
    {
        /// <summary>
        /// Moves the reading cursor to the next entity or row.
        /// </summary>
        Task<bool> MoveNextAsync();
    }
}