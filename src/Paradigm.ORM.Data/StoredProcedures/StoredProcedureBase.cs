using System;
using System.Data;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides a base class for different types of stored procedures.
    /// </summary>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.IRoutine" />
    public abstract class StoredProcedureBase<TParameters> : IRoutine
    {
        #region Properties

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        protected IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Gets the database connector.
        /// </summary>
        private IDatabaseConnector Connector { get; set; }

        /// <summary>
        /// Gets the table type descriptor.
        /// </summary>
        private IRoutineTypeDescriptor Descriptor { get; set; }

        /// <summary>
        /// Gets the routine caller command.
        /// </summary>
        public IDatabaseCommand Command { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureBase{TParameters}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        protected StoredProcedureBase(IServiceProvider serviceProvider): this(serviceProvider, serviceProvider.GetService<IDatabaseConnector>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureBase{TParameters}"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        protected StoredProcedureBase(IDatabaseConnector connector): this(null, connector)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureBase{TParameters}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        private StoredProcedureBase(IServiceProvider serviceProvider, IDatabaseConnector connector)
        {
            this.ServiceProvider = serviceProvider;
            this.Connector = connector;

            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Command?.Dispose();

            this.Connector = null;
            this.Command = null;
            this.ServiceProvider = null;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Sets the  command parameters using the parameter property values.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        protected void SetParametersValue(TParameters parameters)
        {
            for (var i = 0; i < this.Descriptor.Parameters.Count; i++)
            {
                var parameter = this.Descriptor.Parameters[i];
                var commandParameter = this.Command.GetParameter(i);
                
                commandParameter.Value = parameter.PropertyInfo.GetValue(parameters) ?? DBNull.Value;
            }
        }

        /// <summary>
        /// Executes before the initialization.
        /// </summary>
        protected virtual void BeforeInitialize()
        {
        }

        /// <summary>
        /// Executes after the initialization.
        /// </summary>
        protected virtual void AfterInitialize()
        {
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the stored procedure and all its dependant objects.
        /// </summary>
        private void Initialize()
        {
            this.Connector = this.Connector ?? this.ServiceProvider?.GetService<IDatabaseConnector>();

            if (this.Connector == null)
                throw new OrmConnectorException("The stored procedure can not be initialized because the connector couldn't be resolved.");

            this.BeforeInitialize();

            this.Descriptor = new RoutineTypeDescriptor(typeof(TParameters));

            this.Command = this.Connector.CreateCommand<TParameters>(this.GetRoutineName(), CommandType.StoredProcedure, this.Descriptor);

            this.AfterInitialize();
        }

        /// <summary>
        /// Gets the name of the Routine already scaped.
        /// </summary>
        /// <returns>The name of the Routine.</returns>
        protected string GetRoutineName()
        {
            var builder = new StringBuilder();
            var formatProvider = this.Connector.GetCommandFormatProvider();

            if (!string.IsNullOrWhiteSpace(this.Descriptor.CatalogName))
                builder.AppendFormat("{0}.", formatProvider.GetEscapedName(this.Descriptor.CatalogName));

            if (!string.IsNullOrWhiteSpace(this.Descriptor.SchemaName))
                builder.AppendFormat("{0}.", formatProvider.GetEscapedName(this.Descriptor.SchemaName));

            builder.Append(formatProvider.GetEscapedName(this.Descriptor.RoutineName));

            return builder.ToString();
        }

        #endregion
    }
}