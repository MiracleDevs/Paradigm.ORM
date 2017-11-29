using System;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

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
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the database connector.
        /// </summary>
        protected IDatabaseConnector Connector { get; }

        /// <summary>
        /// Gets the format provider.
        /// </summary>
        private ICommandFormatProvider FormatProvider { get; }

        /// <summary>
        /// Gets the table type descriptor.
        /// </summary>
        private IRoutineTypeDescriptor Descriptor { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureBase{TParameters}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        protected StoredProcedureBase(IServiceProvider serviceProvider) : this(serviceProvider, serviceProvider.GetService<IDatabaseConnector>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureBase{TParameters}"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        protected StoredProcedureBase(IDatabaseConnector connector) : this(null, connector)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureBase{TParameters}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        protected StoredProcedureBase(IServiceProvider serviceProvider, IDatabaseConnector connector)
        {
            this.ServiceProvider = serviceProvider;
            this.Connector = connector;
            this.Descriptor = DescriptorCache.Instance.GetRoutineTypeDescriptor(typeof(TParameters));
            this.FormatProvider = this.Connector.GetCommandFormatProvider();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Populates the stored procedure command parameters.
        /// </summary>
        /// <param name="command">A database command.</param>
        /// <param name="parameters">The parameters.</param>
        protected void PopulateParameters(IDatabaseCommand command, TParameters parameters)
        {
            var typeConverter = this.Connector.GetDbStringTypeConverter();
            var valueConverter = this.Connector.GetValueConverter();

            foreach (var parameter in this.Descriptor.Parameters)
            {
                command.AddParameter(
                    this.FormatProvider.GetParameterName(parameter.ParameterName),
                    typeConverter.GetType(parameter.DataType),
                    parameter.MaxSize,
                    parameter.Precision,
                    parameter.Scale,
                    valueConverter.ConvertFrom(parameter.PropertyInfo.GetValue(parameters), parameter.DataType));
            }
        }

        /// <summary>
        /// Gets the name of the routine.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetRoutineName()
        {
            return this.FormatProvider.GetRoutineName(this.Descriptor);
        }

        #endregion
    }
}