namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides an interface to execute scalar stored procedures.
    /// </summary>
    /// <remarks>
    /// Instead of sending individual parameters to the procedure, the orm expects a  <see cref="TParameters"/> type
    /// containing or referencing the mapping information, where individual parameters will be mapped to properties.
    /// </remarks>
    /// <typeparam name="TParameters">The type of the parameter class.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.IRoutine" />
    public partial interface IScalarStoredProcedure<in TParameters, TResult> : IRoutine
    {
        /// <summary>
        /// Executes the stored procedure and returns a scalar value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The scalar value.</returns>
        TResult ExecuteScalar(TParameters parameters);
    }
}