using System;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides the means to execute non query stored procedures.
    /// </summary>
    /// <remarks>
    /// Instead of sending individual parameters to the procedure, the orm expects a  <see cref="TParameters"/> type
    /// containing or referencing the mapping information, where individual parameters will be mapped to properties.
    /// </remarks>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.StoredProcedureBase{TParameters}" />
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.INonQueryStoredProcedure{TParameters}" />
    public partial class NonQueryStoredProcedure<TParameters> : StoredProcedureBase<TParameters>, INonQueryStoredProcedure<TParameters>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NonQueryStoredProcedure{TParameters}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public NonQueryStoredProcedure(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonQueryStoredProcedure{TParameters}"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public NonQueryStoredProcedure(IDatabaseConnector connector) : base(connector)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonQueryStoredProcedure{TParameters}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        public NonQueryStoredProcedure(IServiceProvider serviceProvider, IDatabaseConnector connector): base(serviceProvider, connector)
        {           
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the stored procedure as a non query.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Number of affected rows.
        /// </returns>
        public int ExecuteNonQuery(TParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("Must give parameters to execute the stored procedure.");

            this.SetParametersValue(parameters);
            return this.Command.ExecuteNonQuery();
        }

        #endregion
    }
}