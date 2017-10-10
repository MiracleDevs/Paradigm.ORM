using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Converters;
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
    /// Basically contains a table type descriptor, a command and a database reader mapper.
    /// The basic idea behind a query is to facilitate the creation of queries, and the ability
    /// to reuse them if neccessary.
    /// The difference between a query and a custom query, is that a custom query does not uses
    /// the default select command, but allows the user to provide a custom select to the database.
    /// But even with a custom query string, the custom query requires the table type descriptor to
    /// known how to map the results.
    /// </remarks>
    /// <remarks>
    /// The <see cref="Paradigm.ORM.Data.Database.IDatabaseConnector"/> contains extensions to create
    /// query objects in a single method call. See <see cref="Paradigm.ORM.Data.Extensions.DatabaseConnectorExtensions.Query{TResultType}"/>
    /// </remarks>
    /// <typeparam name="TResultType">The type containing or referencing the mapping information, that will be returned after executing the query.</typeparam>
    /// <seealso cref="IQuery{TResultType}" />
    public partial class CustomQuery<TResultType> : IQuery<TResultType>
        where TResultType : class, new()
    {
        #region Properties

        /// <summary>
        /// Gets the command text.
        /// </summary>
        protected string CommandText { get; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        protected IDatabaseCommand Command { get; set; }

        /// <summary>
        /// Gets the database connector.
        /// </summary>
        protected IDatabaseConnector Connector { get; private set; }

        /// <summary>
        /// Gets the custom type descriptor.
        /// </summary>
        protected ICustomTypeDescriptor Descriptor { get; private set; }

        /// <summary>
        /// Gets the database reader mapper.
        /// </summary>
        protected IDatabaseReaderMapper<TResultType> Mapper { get; private set; }

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomQuery{TResultType}"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="query">The query.</param>
        public CustomQuery(IDatabaseConnector connector, string query) : this(connector, query, new CustomTypeDescriptor(typeof(TResultType)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomQuery{TResultType}"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="query">The query.</param>
        /// <param name="descriptor">The custom type descriptor.</param>
        public CustomQuery(IDatabaseConnector connector, string query, ICustomTypeDescriptor descriptor)
        {
            this.Connector = connector;
            this.Descriptor = descriptor;
            this.CommandText = query;
            this.Command = connector.CreateCommand();
            this.Command.CommandText = query;
            this.Mapper = new DatabaseReaderMapper<TResultType>(descriptor);
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
            this.Descriptor = null;
            this.Command = null;
            this.Mapper = null;
        }

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
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                this.Command.CommandText = $"{this.CommandText} WHERE {whereClause}";

                this.Command.ClearParameters();

                if (parameters != null)
                {
                    for (var index = 0; index < parameters.Length; index++)
                    {
                        var parameter = parameters[index];
                        var type = parameter == null ? typeof(object) : parameter.GetType();
                        var commandParameter = this.Command.AddParameter($"@{index + 1}", DbTypeConverter.FromType(type));
                        commandParameter.Value = parameter ?? DBNull.Value;
                    }
                }
            }

            return this.Connector.ExecuteReader(this.Command, reader => this.Mapper.Map(reader));
        }

        #endregion
    }
}