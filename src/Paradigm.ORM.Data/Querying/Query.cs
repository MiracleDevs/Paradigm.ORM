using System.Collections.Generic;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Mappers.Generic;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.Querying
{
    /// <summary>
    /// Provides the means to execute query objects.
    /// </summary>
    /// <remarks>
    /// A query object encapsulates all the steps in a data reader operation.
    /// Basically contains a table type descriptor, a select command and a database reader mapper.
    /// The basic idea behind a query is to facilitate the creation of queries, and the ability
    /// to reuse them if necessary.
    /// </remarks>
    /// <remarks>
    /// The <see cref="IDatabaseConnector"/> contains extensions to create
    /// query objects in a single method call. See <see cref="DatabaseConnectorExtensions.Query{TResultType}"/>
    /// </remarks>
    /// <typeparam name="TResultType">The type containing or referencing the mapping information, that will be returned after executing the query.</typeparam>
    /// <seealso cref="IQuery{TResultType}" />
    public partial class Query<TResultType> : IQuery<TResultType>
        where TResultType : class
    {
        #region Properties

        /// <summary>
        /// Gets the select command builder.
        /// </summary>
        protected ISelectCommandBuilder SelectCommandBuilder { get; }

        /// <summary>
        /// Gets the database connector.
        /// </summary>
        protected IDatabaseConnector Connector { get; }

        /// <summary>
        /// Gets the table type descriptor.
        /// </summary>
        protected ITableTypeDescriptor Descriptor { get; }

        /// <summary>
        /// Gets or sets the database reader mapper.
        /// </summary>
        protected IDatabaseReaderMapper<TResultType> Mapper { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TResultType}"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public Query(IDatabaseConnector connector)
        {
            this.Connector = connector;
            this.Descriptor = DescriptorCache.Instance.GetTableTypeDescriptor(typeof(TResultType));
            this.SelectCommandBuilder = connector.GetCommandBuilderFactory().CreateSelectCommandBuilder(this.Descriptor);
            this.Mapper = new DatabaseReaderMapper<TResultType>(connector, this.Descriptor);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the specified query and returns a list of <see cref="TResultType" />.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>
        /// A list of <see cref="TResultType" />.
        /// </returns>
        public List<TResultType> Execute(string whereClause = null, params object[] parameters)
        {
            using var command = this.SelectCommandBuilder.GetCommand(whereClause, parameters);
            return this.Connector.ExecuteReader(command, reader => this.Mapper.Map(reader));
        }

        #endregion
    }
}