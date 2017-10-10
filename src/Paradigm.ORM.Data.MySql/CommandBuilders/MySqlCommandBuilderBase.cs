using System.Text;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.MySql.CommandBuilders
{
    /// <summary>
    /// Provide base methods and functionality for all the command builders.
    /// </summary>
    public abstract class MySqlCommandBuilderBase: CommandBuilderBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlCommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        protected MySqlCommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the name of the table already scaped.
        /// </summary>
        /// <returns>The name of the table.</returns>
        protected override string GetTableName()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // overrides the base method to prevent the schema info to be rendered.
            // MySql database does not have 3 level entities, only schema (catalog) and tables.
            ////////////////////////////////////////////////////////////////////////////////////////
            
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.Descriptor.CatalogName))
                builder.AppendFormat((string) "{0}.", (object) this.FormatProvider.GetEscapedName(this.Descriptor.CatalogName));

            builder.Append((string) this.FormatProvider.GetEscapedName(this.Descriptor.TableName));

            return builder.ToString();
        }

        #endregion
    }
}