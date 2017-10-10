namespace Paradigm.ORM.Data.Database.Schema.Structure
{
    /// <summary>
    /// Specifies what a stored procedure will return.
    /// </summary>
    public enum StoredProcedureType
    {
        /// <summary>
        /// Returns a scalar value.
        /// </summary>
        Scalar,

        /// <summary>
        /// The stored procedure won't reture any data.
        /// </summary>
        NonQuery,

        /// <summary>
        /// The stored procedure will return one or more result rests.
        /// </summary>
        Reader
    }
}