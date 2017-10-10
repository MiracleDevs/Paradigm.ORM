using System;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides the means to execute scalar stored procedures.
    /// </summary>
    /// <remarks>
    /// Instead of sending individual parameters to the procedure, the orm expects a  <see cref="TParameters"/> type
    /// containing or referencing the mapping information, where individual parameters will be mapped to properties.
    /// </remarks>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.StoredProcedureBase{TParameters}" />
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.IScalarStoredProcedure{TParameters, TResult}" />
    public partial class ScalarStoredProcedure<TParameters, TResult> : StoredProcedureBase<TParameters>, IScalarStoredProcedure<TParameters, TResult>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarStoredProcedure{TParameters, TResult}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ScalarStoredProcedure(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarStoredProcedure{TParameters, TResult}"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public ScalarStoredProcedure(IDatabaseConnector connector) : base(connector)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the stored procedure and returns a scalar value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The scalar value.
        /// </returns>
        public TResult ExecuteScalar(TParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("Must give parameters to execute the stored procedure.");

            this.SetParametersValue(parameters);
            return (TResult)this.Command.ExecuteScalar();
        }

        #endregion
    }
}