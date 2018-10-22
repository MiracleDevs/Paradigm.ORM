using System.Linq;
using System.Text;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql delete command builder objects.
    /// </summary>
    /// <seealso cref="CommandBuilderBase" />
    /// <seealso cref="IDeleteCommandBuilder" />
    public abstract class DeleteCommandBuilderBase : CommandBuilderBase, IDeleteCommandBuilder
    {
        #region Columns

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        protected string CommandText { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        protected DeleteCommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a delete command query ready to delete one or more entities.
        /// </summary>
        /// <param name="valueProvider"></param>
        /// <returns>
        /// A delete command already parametrized to delete the entities.
        /// </returns>
        public IDatabaseCommand GetCommand(IValueProvider valueProvider)
        {
            var typeConverter = this.Connector.GetDbStringTypeConverter();
            var command = this.Connector.CreateCommand(this.CommandText);

            foreach (var column in this.Descriptor.PrimaryKeyColumns)
            {
                command.AddParameter(
                    this.FormatProvider.GetParameterName(column.ColumnName),
                    typeConverter.GetType(column.DataType),
                    column.MaxSize,
                    column.Precision,
                    column.Scale,
                    valueProvider.GetValue(column));
            }

            return command;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Initializes the command builder.
        /// </summary>
        private void Initialize()
        {
            if (!this.Descriptor.PrimaryKeyColumns.Any())
                throw new OrmNoPrimaryKeysException(this.Descriptor.TableName);

            var builder = new StringBuilder();

            builder.AppendFormat("DELETE FROM {0} WHERE ", this.FormatProvider.GetTableName(this.Descriptor));

            foreach (var primaryKey in this.Descriptor.PrimaryKeyColumns)
            {
                var columnName = primaryKey.ColumnName;

                builder.AppendFormat("{0}={1} AND ", 
                    this.FormatProvider.GetEscapedName(columnName), 
                    this.FormatProvider.GetParameterName(columnName));
            }

            this.CommandText = builder.Remove(builder.Length - 5, 5).ToString();
        }

        #endregion
    }
}