using System;
using System.Collections.Generic;
using System.Data;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Mappers.Generic;

namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides the means to execute data reader stored procedures returning only 1 result set.
    /// </summary>
    /// <remarks>
    /// Instead of sending individual parameters to the procedure, the orm expects a  <see cref="TParameters"/> type
    /// containing or referencing the mapping information, where individual parameters will be mapped to properties.
    /// </remarks>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.StoredProcedureBase{TParameters}" />
    /// <seealso cref="IReaderStoredProcedure{TParameters,TResult1}" />
    public partial class ReaderStoredProcedure<TParameters, TResult>:
        StoredProcedureBase<TParameters>,
        IReaderStoredProcedure<TParameters, TResult>
    {
        #region Properties

        /// <summary>
        /// Gets the result mapper.
        /// </summary>
        private IDatabaseReaderMapper<TResult> Mapper { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ReaderStoredProcedure.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ReaderStoredProcedure(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the ReaderStoredProcedure.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public ReaderStoredProcedure(IDatabaseConnector connector) : base(connector)
        {
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderStoredProcedure{TParameters, TResult}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        public ReaderStoredProcedure(IServiceProvider serviceProvider, IDatabaseConnector connector): base(serviceProvider, connector)
        {
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the ReaderStoredProcedure.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="mapper">The result mapper.</param>
        public ReaderStoredProcedure(IDatabaseConnector connector, IDatabaseReaderMapper<TResult> mapper) : base(connector)
        {
            this.Mapper = mapper;
        }

        /// <summary>
        /// Initializes a new instance of the ReaderStoredProcedure.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        /// <param name="mapper">The result mapper.</param>
        public ReaderStoredProcedure(
            IServiceProvider serviceProvider,
            IDatabaseConnector connector,
            IDatabaseReaderMapper<TResult> mapper) : base(serviceProvider, connector)
        {
            this.Mapper = mapper;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the stored procedure and return a list of <see cref="TResult" />.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// List of <see cref="TResult" />
        /// </returns>
        public List<TResult> Execute(TParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters), "Must give parameters to execute the stored procedure.");

            using var command = this.Connector.CreateCommand(this.GetRoutineName(), CommandType.StoredProcedure);
            this.PopulateParameters(command, parameters);

            using var reader = command.ExecuteReader();
            return this.Mapper.Map(reader);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Executes after the initialization.
        /// </summary>
        protected void Initialize()
        {
            this.Mapper ??= this.ServiceProvider.GetServiceIfAvailable<IDatabaseReaderMapper<TResult>>(() => new DatabaseReaderMapper<TResult>(this.Connector));

            if (this.Mapper == null)
                throw new OrmException("The mapper can not be null.");
        }

        #endregion
    }
}