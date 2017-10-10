namespace Paradigm.ORM.Data.Database
{
    /// <summary>
    /// Provides an interface to retrieve data type value minimum and maximums.
    /// </summary>
    public interface IDbTypeValueRangeProvider
    {
        /// <summary>
        /// Gets the minimum value for a given sql type.
        /// </summary>
        /// <remarks>
        /// The expected types may vary between database implementations.
        /// </remarks>
        /// <param name="dataType">Name of the sql type.</param>
        /// <returns>The minimum value possible for a given type; otherwise null.</returns>
        object GetMinValue(string dataType);

        /// <summary>
        /// Gets the maximum value for a given sql type.
        /// </summary>
        /// <remarks>
        /// The expected types may vary between database implementations.
        /// </remarks>
        /// <param name="dataType">Name of the sql type.</param>
        /// <returns>The maximum value possible for a given type; otherwise null.</returns>
        object GetMaxValue(string dataType);
    }
}