using System;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.ValueProviders;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.Cassandra.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql insert command builder objects.
    /// </summary>
    /// <seealso cref="CqlCommandBuilderBase" />
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.IInsertCommandBuilder" />
    public class CqlInsertCommandBuilder : CqlCommandBuilderBase, IInsertCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlInsertCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public CqlInsertCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
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
                this.Command.GetParameter(i).Value = valueProvider.GetValue(this.Descriptor.SimpleColumns[i]) ?? DBNull.Value;
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