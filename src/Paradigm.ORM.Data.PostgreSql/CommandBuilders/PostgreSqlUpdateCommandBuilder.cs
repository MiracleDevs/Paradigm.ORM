using System;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.PostgreSql.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for postgresql update command builder objects.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.CommandBuilderBase" />
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.IUpdateCommandBuilder" />
    public class PostgreSqlUpdateCommandBuilder : CommandBuilderBase, IUpdateCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlUpdateCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public PostgreSqlUpdateCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an update command query ready to update one entity.
        /// </summary>
        /// <param name="valueProvider"></param>
        /// <returns>
        /// An update command already parametrized to update the entity.
        /// </returns>
        public IDatabaseCommand GetCommand(IValueProvider valueProvider)
        {
            for (var i = 0; i < this.Descriptor.AllColumns.Count; i++)
            {
                this.Command.GetParameter(i).Value = valueProvider.GetValue(this.Descriptor.AllColumns[i]);
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
            var properties = this.Descriptor.AllColumns;
            this.Command = this.Connector.CreateCommand($"UPDATE {this.GetTableName()} SET {this.GetDbParameterNamesAndValues(properties)} WHERE {this.GetDbParameterNamesAndValues(this.Descriptor.PrimaryKeyColumns, " AND ")}");
            this.PopulateParameters(properties);
        }

        #endregion
    }
}