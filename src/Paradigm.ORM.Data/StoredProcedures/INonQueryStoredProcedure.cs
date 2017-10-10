namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides an interface to execute non query stored procedures.
    /// </summary>
    /// <remarks>
    /// Instead of sending individual parameters to the procedure, the orm expects a  <see cref="TParameters"/> type
    /// containing or referencing the mapping information, where individual parameters will be mapped to properties.
    /// </remarks>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.IRoutine" />
    public partial interface INonQueryStoredProcedure<in TParameters> : IRoutine
    {
        /// <summary>
        /// Executes the stored procedure as a non query.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Number of affected rows.</returns>
        int ExecuteNonQuery(TParameters parameters);
    }
}