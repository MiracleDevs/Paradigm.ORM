using System;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.PostgreSql.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for postgresql insert command builder objects.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.CommandBuilderBase" />
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.IInsertCommandBuilder" />
    public class PostgreSqlInsertCommandBuilder : CommandBuilderBase, IInsertCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlInsertCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public PostgreSqlInsertCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an insert command query ready to insert one entity.
        /// </summary>
        /// <param name="valueProvider"></param>
        /// <returns>
        /// An insert command already parametrized to insert the entity.
        /// </returns>
        public IDatabaseCommand GetCommand(IValueProvider valueProvider)
        {
            for (var i = 0; i < this.Descriptor.SimpleColumns.Count; i++)
            {
                this.Command.GetParameter(i).Value = valueProvider.GetValue(this.Descriptor.SimpleColumns[i]);
            }

            return this.Command;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the command builder.
        /// </summary>
        private void Initialize()
        {
            var properties = this.Descriptor.SimpleColumns;
            this.Command = this.Connector.CreateCommand($"INSERT INTO {this.GetTableName()} ({this.GetPropertyNames(properties)}) VALUES ({this.GetDbParameterNames(properties)})");
            this.PopulateParameters(properties);
        }

        #endregion
    }
}